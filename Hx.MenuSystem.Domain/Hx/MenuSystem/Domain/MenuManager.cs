using Volo.Abp;
using Volo.Abp.Domain.Services;

namespace Hx.MenuSystem.Domain
{
    // 领域服务
    public class MenuManager(IMenuRepository menuRepository) : DomainService
    {
        private readonly IMenuRepository _menuRepository = menuRepository;

        public async Task<Menu> CreateAsync(
            string name,
            string displayName,
            string route,
            string appName,
            string permissionName,
            string? icon,
            double order,
            bool isActive,
            Guid? parentId = null,
            Guid? appFormId = null)
        {
            await ValidateNameUniquenessAsync(name, parentId);

            return new Menu(
                GuidGenerator.Create(),
                name,
                displayName,
                route,
                appName,
                permissionName,
                icon,
                order,
                isActive,
                parentId,
                appFormId
            );
        }

        private async Task ValidateNameUniquenessAsync(string name, Guid? parentId)
        {
            var existing = await _menuRepository.FindByNameAsync(name, parentId);
            if (existing != null)
            {
                throw new BusinessException("DynamicMenu:MenuNameAlreadyExists")
                    .WithData("Name", name);
            }
        }
    }
}