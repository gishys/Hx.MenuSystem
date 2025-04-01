using Hx.MenuSystem.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Hx.MenuSystem.Migrations
{
    public class MenuSystemMigrationsContext(DbContextOptions<MenuSystemMigrationsContext> options) : AbpDbContext<MenuSystemMigrationsContext>(options)
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.OnModuleConfigration();
        }
    }
}
