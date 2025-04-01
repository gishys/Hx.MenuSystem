using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Hx.MenuSystem.EntityFrameworkCore
{
    [ConnectionStringName("MenuSystem")]
    public class MenuSystemDbContext(DbContextOptions<MenuSystemDbContext> options) : AbpDbContext<MenuSystemDbContext>(options)
    {
        public virtual DbSet<Menu> Menus { get; set; }
        public virtual DbSet<UserMenu> UserMenus { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<Menu>(b =>
            {
                b.ToTable("SYS_MENUS");
                b.Property(x => x.Id).HasColumnName("ID");
                b.Property(x => x.TenantId).HasColumnName("TENANTID");
                b.Property(x => x.Name).IsRequired().HasMaxLength(MenuConsts.MaxNameLength).HasColumnName("NAME");
                b.Property(x => x.DisplayName).IsRequired().HasMaxLength(MenuConsts.MaxDisplayNameLength).HasColumnName("DISPLAY_NAME");
                b.Property(x => x.Route).IsRequired().HasMaxLength(MenuConsts.MaxRouteLength).HasColumnName("ROUTE");
                b.Property(x => x.AppName).IsRequired().HasMaxLength(MenuConsts.MaxAppNameLength).HasColumnName("APP_NAME");
                b.Property(x => x.PermissionName).IsRequired().HasMaxLength(MenuConsts.MaxPermissionNameLength).HasColumnName("PERMISSION_NAME");
                b.Property(x => x.Icon).HasMaxLength(MenuConsts.MaxIconLength).HasColumnName("ICON");
                b.Property(x => x.Order).IsRequired().HasColumnName("ORDER");
                b.Property(x => x.ParentId).HasColumnName("PARENT_ID");
                b.Property(x => x.AppFormId).HasColumnName("APP_FORM_ID");
                b.Property(x => x.IsActive).IsRequired().HasColumnName("IS_ACTIVE");
            });

            modelBuilder.Entity<UserMenu>(b =>
            {
                b.ToTable("SYS_USERMENUS");
                b.HasKey(x => new { x.UserId, x.MenuId });
            });
        }
    }
}
