using Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Hx.MenuSystem.Domain.Shared
{
    [DependsOn(typeof(AbpLocalizationModule))]
    public class MenuSystemDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MenuSystemDomainSharedModule>("Hx.MenuSystem.Domain.Shared");
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MenuResource>("en")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization");
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("Hx.MenuSystem.Domain.Shared", typeof(MenuResource));
            });
        }
    }
}
