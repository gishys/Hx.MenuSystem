using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.Extensions.Options;
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
            var permissionNames = input.Permissions.Select(p => p.Name).Distinct().ToList();
            if (permissionNames == null)
                throw new ArgumentNullException(nameof(permissionNames));
            var menus = await _menuRepository.FindByPermissionNamesAsync(permissionNames.ToArray());
            var menuDict = menus.ToDictionary(m => m.PermissionName, m => m);
            var subjectId = new Guid(providerKey);
            foreach (var permissionDto in input.Permissions)
            {
                if (!menuDict.TryGetValue(permissionDto.Name, out var menu))
                    continue;
                if (permissionDto.IsGranted)
                {
                    menu.AddOrUpdateSubject(subjectId, providerName.ToSubjectType());
                }
                else
                {
                    menu.Subjects.RemoveAll(u => u.SubjectId == subjectId);
                }
            }
            await _menuRepository.UpdateManyAsync(menus);
            await CurrentUnitOfWork.SaveChangesAsync();
            await base.UpdateAsync(providerName, providerKey, input);
        }
    }
}