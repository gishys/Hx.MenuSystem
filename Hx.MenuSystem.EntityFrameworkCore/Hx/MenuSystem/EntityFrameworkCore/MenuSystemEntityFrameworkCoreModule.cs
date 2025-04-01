using Hx.MenuSystem.Domain;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.PostgreSql;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.EntityFrameworkCore
{
    [DependsOn(typeof(AbpEntityFrameworkCoreModule))]
    [DependsOn(typeof(AbpEntityFrameworkCorePostgreSqlModule))]
    internal class MenuSystemEntityFrameworkCoreModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
            context.Services.AddAbpDbContext<MenuSystemDbContext>(options =>
            {
                options.AddDefaultRepositories(includeAllEntities: true);
            });
            context.Services.AddAbpDbContext<MenuSystemDbContext>(options =>
            {
                options.AddRepository<Menu, EfCoreMenuRepository>();
            });
            Configure<AbpDbContextOptions>(options =>
            {
                options.UseNpgsql(options =>
                {
                });
            });
        }
    }
}
