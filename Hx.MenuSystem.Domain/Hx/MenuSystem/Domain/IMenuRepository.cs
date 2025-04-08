using Volo.Abp.Domain.Repositories;

namespace Hx.MenuSystem.Domain
{
    public interface IMenuRepository : IBasicRepository<Menu, Guid>
    {
        Task<List<Menu>> GetListBySubjectIdAsync(string subjectId, Guid? tenantId);
        Task<Menu?> FindByNameAsync(string name, Guid? tenantId);
        Task<List<Menu>> FindByAppNameAsync(string appName, string? displayName, Guid? tenantId);
        Task<List<Menu>> FindByIdsAsync(Guid[] ids);
        Task<List<Menu>> FindByPermissionNamesAsync(string[] names);
        Task<Menu?> FindByPermissionNameAsync(string permissionName, Guid? tenantId);
    }
}
