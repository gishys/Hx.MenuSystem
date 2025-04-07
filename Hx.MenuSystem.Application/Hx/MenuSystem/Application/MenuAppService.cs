using Hx.MenuSystem.Application.Contracts;
using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace Hx.MenuSystem.Application
{
    [Authorize]
    public class MenuAppService(
        IMenuRepository menuRepository,
        MenuManager menuManager,
        ICurrentUser currentUser,
        IServiceProvider serviceProvider) : ApplicationService, IMenuAppService
    {
        private readonly IMenuRepository _menuRepository = menuRepository;
        private readonly MenuManager _menuManager = menuManager;
        private readonly ICurrentUser _currentUser = currentUser;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<List<MenuDto>> GetCurrentUserMenusAsync(bool checkAuth = true)
        {
            var userId = _currentUser.GetId();
            var menus = await _menuRepository.GetListBySubjectIdAsync(userId);
            var menuAuths = checkAuth ? await CheckAuthAsync(menus) : menus;
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menuAuths);
            return ConvertToMenuTree(menuDtos);
        }
        [Authorize("MenuSystem")]
        public async Task<List<MenuDto>> GetMenusByAppNameAsync(string appName, string? displayName = null, bool checkAuth = true)
        {
            var menus = await _menuRepository.FindByAppNameAsync(appName, displayName, CurrentTenant.Id);
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
            if (checkAuth)
            {
                var (filteredMenus, grantedMenuIds) = await CheckAuthAsync(menuDtos);
                menuDtos = filteredMenus;
                if (grantedMenuIds.Count > 0)
                {
                    await AddMenuUsersAsync(new CreateOrUpdateMenuSubjectDto() { MenuIds = [.. grantedMenuIds], SubjectId = CurrentUser.GetId(), SubjectType = "U" });
                }
            }
            menuDtos = ConvertToMenuTree(menuDtos);
            return menuDtos;
        }
        [Authorize("MenuSystem.GrantedAuth")]
        public async Task<List<MenuDto>> AddMenuUsersAsync(CreateOrUpdateMenuSubjectDto input)
        {
            var menus = await _menuRepository.FindByIdsAsync(input.MenuIds);
            var foundMenuIds = menus.Select(m => m.Id).ToHashSet();
            var missingMenuIds = input.MenuIds.Except(foundMenuIds).ToList();
            if (missingMenuIds.Count > 0)
            {
                throw new EntityNotFoundException($"未找到ID为 {string.Join(", ", missingMenuIds)} 的菜单");
            }
            foreach (var menu in menus)
            {
                menu.AddOrUpdateSubject(input.SubjectId, input.SubjectType.ToSubjectType());
            }
            await _menuRepository.UpdateManyAsync(menus);
            return ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
        }
        [Authorize("MenuSystem.GrantedAuth")]
        public async Task PutMenuUsersAsync(CreateOrUpdateMenuSubjectDto input)
        {
            var menus = await _menuRepository.FindByIdsAsync(input.MenuIds);
            var foundMenuIds = menus.Select(m => m.Id).ToHashSet();
            var missingMenuIds = input.MenuIds.Except(foundMenuIds).ToList();
            if (missingMenuIds.Count > 0)
            {
                throw new EntityNotFoundException($"未找到ID为 {string.Join(", ", missingMenuIds)} 的菜单");
            }
            foreach (var menu in menus)
            {
                menu.Subjects.RemoveAll(r => r.SubjectId == input.SubjectId);
            }
            await _menuRepository.UpdateManyAsync(menus);
        }
        private static List<MenuDto> ConvertToMenuTree(List<MenuDto> menuDtos)
        {
            var lookup = menuDtos.ToLookup(m => m.ParentId);
            foreach (var menu in menuDtos)
            {
                menu.Children = [.. lookup[menu.Id].OrderBy(m => m.Order)];
            }
            return [.. lookup[null].OrderBy(m => m.Order)];
        }
        private async Task<List<Menu>> CheckAuthAsync(List<Menu> menus)
        {
            // 获取权限服务并验证依赖
            var permissionService = _serviceProvider.GetService<IPermissionAppService>()
                ?? throw new UserFriendlyException("[IPermissionAppService]未注册权限服务！");
            // 获取当前用户并验证用户状态
            var userId = CurrentUser.Id
                ?? throw new UserFriendlyException("获取当前登录人失败！");
            // 并行获取用户权限和角色权限
            var userPermissionNames = await GetGrantedPermissionNamesAsync(permissionService, "U", userId.ToString());
            var grantedPermissions = new HashSet<string>(userPermissionNames);
            // 并行处理所有角色权限请求
            var rolePermissionTasks = CurrentUser.Roles
                .Select(role => GetGrantedPermissionNamesAsync(permissionService, "R", role));
            foreach (var rolePermissions in await Task.WhenAll(rolePermissionTasks))
            {
                grantedPermissions.UnionWith(rolePermissions);
            }
            // 使用HashSet进行高效权限过滤
            return menus
                .Where(m => !string.IsNullOrEmpty(m.PermissionName) && grantedPermissions.Contains(m.PermissionName))
                .ToList();
        }
        private async Task<(List<MenuDto> FilteredMenus, List<Guid> GrantedMenuIds)> CheckAuthAsync(List<MenuDto> menus)
        {
            var permissionService = _serviceProvider.GetService<IPermissionAppService>()
                ?? throw new UserFriendlyException("[IPermissionAppService]未注册权限服务！");
            var userId = CurrentUser.Id ?? throw new UserFriendlyException("获取当前登录人失败！");
            var userPermissionNames = await GetGrantedPermissionNamesAsync(permissionService, "U", userId.ToString());
            var grantedPermissions = new HashSet<string>(userPermissionNames);
            var rolePermissionTasks = CurrentUser.Roles
                .Select(role => GetGrantedPermissionNamesAsync(permissionService, "R", role));
            foreach (var rolePermissions in await Task.WhenAll(rolePermissionTasks))
            {
                grantedPermissions.UnionWith(rolePermissions);
            }
            var grantedMenuIds = new List<Guid>();
            foreach (var menu in menus)
            {
                if (menu.Children != null && menu.Children.Count > 0)
                {
                    var (filteredChildren, childGrantedIds) = await CheckAuthAsync(menu.Children);
                    menu.Children = filteredChildren;
                    grantedMenuIds.AddRange(childGrantedIds);
                }
                menu.IsGranted = string.IsNullOrEmpty(menu.PermissionName)
                    ? (menu.Children?.Any(c => c.IsGranted) ?? true)
                    : grantedPermissions.Contains(menu.PermissionName);
                if (menu.IsGranted)
                {
                    grantedMenuIds.Add(menu.Id);
                }
            }
            return (menus, grantedMenuIds.Distinct().ToList());
        }
        private static async Task<IEnumerable<string>> GetGrantedPermissionNamesAsync(
            IPermissionAppService permissionService,
            string type,
            string id)
        {
            var permission = await permissionService.GetAsync(type, id);
            return permission.Groups
                .SelectMany(g => g.Permissions)
                .Where(p => p.IsGranted)
                .Select(p => p.Name)
                .Distinct();
        }

        [Authorize("MenuSystem.MenuManagement")]
        public async Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input)
        {
            var menu = await _menuManager.CreateAsync(
                input.Name,
                input.DisplayName,
                input.Route,
                input.AppName,
                input.PermissionName,
                input.Icon,
                input.Order,
                input.IsActive,
                input.ParentId,
                input.AppFormId
            );

            menu.Order = input.Order;
            await _menuRepository.InsertAsync(menu);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }
        public async Task DeleteAsync(Guid id)
        {
            await _menuRepository.DeleteAsync(id);
        }
        [Authorize("MenuSystem.MenuManagement")]
        public async Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input)
        {
            var entity = await _menuRepository.GetAsync(id);
            if (!string.Equals(entity.Name, input.Name))
            {
                entity.SetName(input.Name);
            }
            if (!string.Equals(entity.DisplayName, input.DisplayName))
            {
                entity.SetDisplayName(input.DisplayName);
            }
            if (!string.Equals(entity.Route, input.Route))
            {
                entity.SetRoute(input.Route);
            }
            if (!string.Equals(entity.AppName, input.AppName))
            {
                entity.SetAppName(input.AppName);
            }
            if (!string.Equals(entity.PermissionName, input.PermissionName))
            {
                entity.SetPermissionName(input.PermissionName);
            }
            if (!string.Equals(entity.Icon, input.Icon))
            {
                entity.SetIcon(input.Icon);
            }
            if (entity.Order != input.Order)
            {
                entity.SetOrder(input.Order);
            }
            if (entity.AppFormId != input.AppFormId)
            {
                entity.SetAppFormId(input.AppFormId);
            }
            if (entity.IsActive != input.IsActive)
            {
                entity.SetIsActive(input.IsActive);
            }

            await _menuRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Menu, MenuDto>(entity);
        }
    }
}