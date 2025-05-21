using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.Extensions.Options;
using Volo.Abp;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.PermissionManagement;
using Volo.Abp.SimpleStateChecking;

namespace Hx.MenuSystem.Application
{
    public class HxPermissionAppService(
        IPermissionManager permissionManager,
        IPermissionDefinitionManager permissionDefinitionManager,
        IOptions<PermissionManagementOptions> options,
        ISimpleStateCheckerManager<PermissionDefinition> simpleStateCheckerManager,
        IMenuRepository menuRepository) :
        PermissionAppService(
            permissionManager,
            permissionDefinitionManager,
            options,
            simpleStateCheckerManager), IPermissionAppService
    {
        private readonly IMenuRepository _menuRepository = menuRepository;
        public override async Task UpdateAsync(string providerName, string providerKey, UpdatePermissionsDto input)
        {
            List<string> permissionNames = input.Permissions.Select(p => p.Name).Distinct().ToList();
            if (permissionNames.Count == 0)
                throw new UserFriendlyException(message: "没有有效的权限名称！");
            var menus = await _menuRepository.FindByPermissionNamesAsync([.. permissionNames]);
            var menuDict = menus.ToDictionary(m => m.PermissionName, m => m);
            foreach (var permissionDto in input.Permissions)
            {
                if (!menuDict.TryGetValue(permissionDto.Name, out var menu))
                    continue;
                if (permissionDto.IsGranted)
                {
                    menu.AddOrUpdateSubject(providerKey, providerName.ToSubjectType());
                }
                else
                {
                    menu.Subjects.RemoveAll(u => u.SubjectId == providerKey);
                }
            }
            await _menuRepository.UpdateManyAsync(menus);
#pragma warning disable CS8602 // 解引用可能出现空引用。
            await CurrentUnitOfWork.SaveChangesAsync();
#pragma warning restore CS8602 // 解引用可能出现空引用。
            await base.UpdateAsync(providerName, providerKey, input);
        }
    }
}