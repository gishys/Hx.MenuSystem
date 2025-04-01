using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Hx.MenuSystem.EntityFrameworkCore;

namespace Hx.MenuSystem.Migrations
{
    internal class MenuSystemContextFactory : IDesignTimeDbContextFactory<MenuSystemDbContext>
    {
        public MenuSystemDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();
            var builder =
                new DbContextOptionsBuilder<MenuSystemDbContext>()
                .UseNpgsql(
                configuration.GetConnectionString("MenuSystem"));
            return new MenuSystemDbContext(builder.Options);
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
