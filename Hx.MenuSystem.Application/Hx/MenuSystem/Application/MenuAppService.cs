using Hx.MenuSystem.Application.Contracts;
using Hx.MenuSystem.Domain;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Services;
using Volo.Abp.Users;

namespace Hx.MenuSystem.Application
{
    [Authorize]
    public class MenuAppService : ApplicationService, IMenuAppService
    {
        private readonly IMenuRepository _menuRepository;
        private readonly MenuManager _menuManager;
        private readonly ICurrentUser _currentUser;

        public MenuAppService(
            IMenuRepository menuRepository,
            MenuManager menuManager,
            ICurrentUser currentUser)
        {
            _menuRepository = menuRepository;
            _menuManager = menuManager;
            _currentUser = currentUser;
        }

        public async Task<List<MenuDto>> GetCurrentUserMenusAsync()
        {
            var userId = _currentUser.GetId();
            var menus = await _menuRepository.GetListByUserIdAsync(userId);
            return ConvertToMenuTree(menus);
        }

        private List<MenuDto> ConvertToMenuTree(List<Menu> menus)
        {
            var menuDtos = ObjectMapper.Map<List<Menu>, List<MenuDto>>(menus);
            var lookup = menuDtos.ToLookup(m => m.ParentId);

            foreach (var menu in menuDtos)
            {
                menu.Children = lookup[menu.Id].OrderBy(m => m.Order).ToList();
            }

            return lookup[null].OrderBy(m => m.Order).ToList();
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
                input.ParentId,
                input.AppFormId
            );

            menu.Order = input.Order;
            await _menuRepository.InsertAsync(menu);

            return ObjectMapper.Map<Menu, MenuDto>(menu);
        }
        [Authorize("MenuSystem.MenuManagement")]
        public async Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input)
        {
            var entity = await _menuRepository.GetAsync(id);
            //    input.Order,
            //    input.ParentId,
            //    input.AppFormId
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
                entity.SetName(input.Name);
            }
            await _menuRepository.UpdateAsync(entity);

            return ObjectMapper.Map<Menu, MenuDto>(entity);
        }
    }
}