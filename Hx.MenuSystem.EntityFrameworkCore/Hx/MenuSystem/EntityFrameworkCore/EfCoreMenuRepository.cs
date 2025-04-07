using Hx.MenuSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Hx.MenuSystem.EntityFrameworkCore
{
    public class EfCoreMenuRepository(IDbContextProvider<MenuSystemDbContext> dbContextProvider) : EfCoreRepository<MenuSystemDbContext, Menu, Guid>(dbContextProvider), IMenuRepository
    {
        public async Task<List<Menu>> GetListBySubjectIdAsync(Guid subjectId)
        {
            var dbContext = await GetDbContextAsync();
            var query = from menu in dbContext.Menus
                        join subjectMenu in dbContext.SubjectMenus on menu.Id equals subjectMenu.MenuId
                        where subjectMenu.SubjectId == subjectId && menu.IsActive
                        orderby menu.Order
                        select menu;

            return await query.ToListAsync();
        }
        public async override Task<Menu> GetAsync(Guid id, bool includeDetails = true, CancellationToken cancellationToken = default)
        {
            var dbContext = await GetDbContextAsync();
            var query = from menu in dbContext.Menus where menu.Id == id select menu;
            return await query.Include(d => d.Subjects).FirstAsync(cancellationToken: cancellationToken);
        }
        public async Task<Menu?> FindByNameAsync(string name, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var query = from menu in dbContext.Menus
                        where menu.Name == name && menu.TenantId == tenantId
                        select menu;
            return await query.FirstOrDefaultAsync();
        }
        public async Task<Menu?> FindByPermissionNameAsync(string permissionName, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var query = from menu in dbContext.Menus
                        where menu.PermissionName == permissionName && menu.TenantId == tenantId
                        select menu;
            return await query.FirstOrDefaultAsync();
        }
        public async Task<List<Menu>> FindByAppNameAsync(string appName, string? displayName, Guid? tenantId)
        {
            var dbContext = await GetDbContextAsync();
            var query = from menu in dbContext.Menus
                        where menu.AppName == appName && menu.TenantId == tenantId
                        select menu;
            query = query.WhereIf(!string.IsNullOrEmpty(displayName), d => d.DisplayName.Contains($"{displayName}"));
            return await query.ToListAsync();
        }
        public async Task<List<Menu>> FindByIdsAsync(Guid[] ids)
        {
            if (ids == null || ids.Length == 0)
            {
                return [];
            }
            var dbContext = await GetDbContextAsync();
            var batchSize = 2000;
            var result = new List<Menu>();
            for (var i = 0; i < ids.Length; i += batchSize)
            {
                var batch = ids.Skip(i).Take(batchSize);
                result.AddRange(await dbContext.Menus.Include(m => m.Subjects).Where(m => batch.Contains(m.Id)).ToListAsync());
            }
            return result;
        }
        public async Task<List<Menu>> FindByPermissionNamesAsync(string[] names)
        {
            if (names == null || names.Length == 0)
            {
                return [];
            }
            var dbContext = await GetDbContextAsync();
            var batchSize = 2000;
            var result = new List<Menu>();
            for (var i = 0; i < names.Length; i += batchSize)
            {
                var batch = names.Skip(i).Take(batchSize);
                result.AddRange(await dbContext.Menus.Include(m => m.Subjects).Where(m => batch.Contains(m.PermissionName)).ToListAsync());
            }
            return result;
        }
    }
}
