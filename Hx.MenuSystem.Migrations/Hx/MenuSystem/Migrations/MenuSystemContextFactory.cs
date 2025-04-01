using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Hx.MenuSystem.EntityFrameworkCore;

namespace Hx.MenuSystem.Migrations
{
    internal class MenuSystemContextFactory : IDesignTimeDbContextFactory<MenuSystemMigrationsContext>
    {
        public MenuSystemMigrationsContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder =
                new DbContextOptionsBuilder<MenuSystemMigrationsContext>()
                .UseNpgsql(
                configuration.GetConnectionString("MenuSystem"));
            return new MenuSystemMigrationsContext(builder.Options);
        }
        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
