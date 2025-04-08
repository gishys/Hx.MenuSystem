using Hx.MenuSystem.Domain;
using Hx.MenuSystem.Domain.Shared;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Hx.MenuSystem.EntityFrameworkCore
{
    public static class DbEntityTypeConfigration
    {
        public static void OnModuleConfigration(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Menu>(b =>
            {
                b.ToTable("SYS_MENUS");

                b.HasIndex(x => x.Name);
                b.HasIndex(x => x.AppName);

                b.ConfigureExtraProperties();
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
                b.Property(x => x.ExtraProperties).HasColumnName("EXTRA_PROPERTIES");
                b.Property(x => x.ConcurrencyStamp).HasColumnName("CONCURRENCY_STAMP");
            });

            modelBuilder.Entity<SubjectMenu>(b =>
            {
                b.ToTable("SYS_SUBJECT_MENUS");
                b.HasIndex(x => x.SubjectId);
                b.HasKey(x => new { x.SubjectId, x.MenuId });
                b.Property(x => x.MenuId).HasColumnName("MENU_ID");
                b.Property(x => x.SubjectId).IsRequired().HasMaxLength(SubjectMenuConsts.MaxSubjectIdLength).HasColumnName("SUBJECT_ID");
                b.Property(x => x.Order).HasColumnName("ORDER");
                b.Property(x => x.SubjectType).HasColumnName("SUBJECTTYPE");
                b.Property(x => x.CreationTime).HasColumnName("CREATIONTIME").HasColumnType("timestamp without time zone");
            });
        }
    }
}