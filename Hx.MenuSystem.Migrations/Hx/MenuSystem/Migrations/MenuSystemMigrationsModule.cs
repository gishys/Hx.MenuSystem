using Hx.MenuSystem.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Migrations
{
    public class MenuSystemMigrationsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAbpDbContext<MenuSystemDbContext>();
        }
    }
}
