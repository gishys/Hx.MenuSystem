using Hx.MenuSystem.Application.Contracts;
using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Users;

namespace Hx.MenuSystem.Application
{
    [Authorize]
    public class MenuAppService(
        IMenuRepository menuRepository,
        MenuManager menuManager,
        IServiceProvider serviceProvider) : ApplicationService, IMenuAppService
    {
        private readonly IMenuRepository _menuRepository = menuRepository;
        private readonly MenuManager _menuManager = menuManager;
        private readonly IServiceProvider _serviceProvider = serviceProvider;

        public async Task<List<MenuDto>> GetCurrentUserMenusAsync(bool checkAuth = true)
        {
            var menus = await _menuRepository.GetListBySubjectIdAsync(CurrentUser.GetId().ToString(), CurrentTenant.Id);
            var menuAuths = checkAuth ? await CheckAuthAsync(menus) : menus;
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menuAuths);
            return ConvertToMenuTree(menuDtos);
        }
        [Authorize("MenuSystem.List")]
        public async Task<List<MenuDto>> GetMenusByAppNameAsync(
            string appName,
            string subjectId,
            SubjectType type = SubjectType.User,
            string? displayName = null,
            bool checkAuth = true)
        {
            var menus = await _menuRepository.FindByAppNameAsync(appName, displayName, CurrentTenant.Id);
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
            if (checkAuth)
            {
                var (filteredMenus, grantedMenuIds) = await CheckAuthAsync(menuDtos, subjectId, type);
                menuDtos = filteredMenus;
                if (grantedMenuIds.Count > 0)
                {
                    var menuSubjectDto = grantedMenuIds.GroupBy(g => new { g.Type, g.Id }).Select(m => new
                    {
                        MenuIds = m.Select(i => i.MenuId).ToArray(),
                        SubjectId = m.Key.Id,
                        SubjectType = m.Key.Type,
                    });
                    foreach (var menuId in menuSubjectDto)
                    {
                        await AddOrRemoveMenuUsersAsync(new CreateOrUpdateMenuSubjectDto()
                        {
                            MenuIds = menuId.MenuIds,
                            SubjectId = menuId.SubjectId,
                            SubjectType = menuId.SubjectType,
                            IsGranted = true
                        });
                    }
                }
            }
            menuDtos = ConvertToMenuTree(menuDtos);
            return menuDtos;
        }
        [Authorize("MenuSystem.GrantedAuth")]
        public async Task<List<MenuDto>> AddOrRemoveMenuUsersAsync(CreateOrUpdateMenuSubjectDto input)
        {
            var menus = await _menuRepository.FindByIdsAsync(input.MenuIds);
            var foundMenuIds = menus.Select(m => m.Id).ToHashSet();
            var missingMenuIds = input.MenuIds.Except(foundMenuIds).ToList();

            if (missingMenuIds.Count > 0)
                throw new EntityNotFoundException($"未找到ID为 {string.Join(", ", missingMenuIds)} 的菜单");
            foreach (var menu in menus)
            {
                if (input.IsGranted)
                    menu.AddOrUpdateSubject(input.SubjectId, input.SubjectType.ToSubjectType());
                else
                    menu.Subjects.RemoveAll(r => r.SubjectId == input.SubjectId);
            }
            await _menuRepository.UpdateManyAsync(menus);
            return ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
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
            var permissionService = _serviceProvider.GetService<IPermissionAppService>()
                ?? throw new UserFriendlyException("[IPermissionAppService]未注册权限服务！");
            var userId = CurrentUser.Id?? throw new UserFriendlyException("获取当前登录人失败！");
            var userPermissionNames = await GetGrantedPermissionNamesAsync(permissionService, "U", userId.ToString());
            var grantedPermissions = new HashSet<string>(userPermissionNames);
            var rolePermissionTasks = CurrentUser.Roles.Select(role => GetGrantedPermissionNamesAsync(permissionService, "R", role));
            foreach (var rolePermissions in await Task.WhenAll(rolePermissionTasks))
            {
                grantedPermissions.UnionWith(rolePermissions);
            }
            return menus
                .Where(m => !string.IsNullOrEmpty(m.PermissionName) && grantedPermissions.Contains(m.PermissionName))
                .ToList();
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
        private async Task<(List<MenuDto> FilteredMenus, List<GrantedMenuInfo> GrantedMenuInfos)> CheckAuthAsync(List<MenuDto> menus, string subjectId, SubjectType type)
        {
            var permissionService = _serviceProvider.GetService<IPermissionAppService>()
                ?? throw new UserFriendlyException("[IPermissionAppService]未注册权限服务！");
            var userManager = _serviceProvider.GetService<IdentityUserManager>()
                ?? throw new UserFriendlyException("[IdentityUserManager]未注册用户管理服务！");
            var grantedPermissions = new Dictionary<string, List<GrantedSource>>();
            IList<string> roles;
            if (type == SubjectType.User)
            {
                var user = await userManager.FindByIdAsync(subjectId) ?? throw new UserFriendlyException($"Id为[{subjectId}]的用户不存在！");
                roles = await userManager.GetRolesAsync(user);
                var userGranted = await GetGrantedPermissionsAsync(permissionService, "U", subjectId);
                foreach (var perm in userGranted)
                {
                    if (!grantedPermissions.ContainsKey(perm.Name))
                        grantedPermissions[perm.Name] = [];
                    grantedPermissions[perm.Name].Add(new GrantedSource { Type = "U", Id = subjectId });
                }
            }
            else if (type == SubjectType.Role)
            {
                roles = [subjectId];
            }
            else
            {
                roles = [];
            }
            var rolePermissionTasks = roles.Select(role => GetGrantedPermissionsAsync(permissionService, "R", role));
            foreach (var rolePermissions in await Task.WhenAll(rolePermissionTasks))
            {
                foreach (var perm in rolePermissions)
                {
                    if (!grantedPermissions.ContainsKey(perm.Name))
                        grantedPermissions[perm.Name] = [];
                    grantedPermissions[perm.Name].Add(new GrantedSource { Type = "R", Id = perm.Id });
                }
            }
            var grantedMenuInfos = new List<GrantedMenuInfo>();
            foreach (var menu in menus)
            {
                if (menu.Children != null && menu.Children.Count > 0)
                {
                    var (filteredChildren, childGranted) = await CheckAuthAsync(menu.Children, subjectId, type);
                    menu.Children = filteredChildren;
                    grantedMenuInfos.AddRange(childGranted);
                }
                menu.IsGranted = string.IsNullOrEmpty(menu.PermissionName)
                    ? (menu.Children?.Any(c => c.IsGranted) ?? true)
                    : grantedPermissions.ContainsKey(menu.PermissionName);

                if (menu.IsGranted)
                {
                    if (!string.IsNullOrEmpty(menu.PermissionName))
                    {
                        foreach (var source in grantedPermissions[menu.PermissionName])
                        {
                            grantedMenuInfos.Add(new GrantedMenuInfo
                            {
                                MenuId = menu.Id,
                                Type = source.Type,
                                Id = source.Id
                            });
                        }
                    }
                }
            }
            return (menus, grantedMenuInfos
                .GroupBy(g => new { g.MenuId, g.Type, g.Id })
                .Select(g => g.First())
                .ToList());
        }
        private static async Task<IEnumerable<PermissionDto>> GetGrantedPermissionsAsync(
            IPermissionAppService permissionService,
            string granteeType,
            string granteeId)
        {
            var result = await permissionService.GetAsync(granteeType, granteeId);
            return result.Groups
                .SelectMany(g => g.Permissions)
                .Where(p => p.IsGranted)
                .Select(p => new PermissionDto { Name = p.Name, Id = granteeId });
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