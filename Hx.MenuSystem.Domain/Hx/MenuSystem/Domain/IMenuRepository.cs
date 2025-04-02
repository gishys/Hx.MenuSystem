using Volo.Abp.Domain.Repositories;

namespace Hx.MenuSystem.Domain
{
    public interface IMenuRepository : IBasicRepository<Menu, Guid>
    {
        Task<List<Menu>> GetListByUserIdAsync(Guid userId);
        Task<Menu?> FindByNameAsync(string name, Guid? tenantId);
        Task<List<Menu>> FindByAppNameAsync(string appName, Guid? tenantId);
    }
}
