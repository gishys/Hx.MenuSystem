using Hx.MenuSystem.Domain;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Hx.MenuSystem.EntityFrameworkCore
{
    [ConnectionStringName("MenuSystem")]
    public class MenuSystemDbContext(DbContextOptions<MenuSystemDbContext> options) : AbpDbContext<MenuSystemDbContext>(options)
    {
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<SubjectMenu> SubjectMenus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.OnModuleConfigration();
        }
    }
}
