﻿using Localization;
using Volo.Abp.Localization;
using Volo.Abp.Localization.ExceptionHandling;
using Volo.Abp.Modularity;
using Volo.Abp.Validation.Localization;
using Volo.Abp.VirtualFileSystem;

namespace Hx.MenuSystem.Domain.Shared
{
    public class MenuSystemDomainSharedModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpVirtualFileSystemOptions>(options =>
            {
                options.FileSets.AddEmbedded<MenuSystemDomainSharedModule>();
            });

            Configure<AbpLocalizationOptions>(options =>
            {
                options.Resources
                    .Add<MenuResource>("zh-Hans")
                    .AddBaseTypes(typeof(AbpValidationResource))
                    .AddVirtualJson("/Localization");

                options.DefaultResourceType = typeof(MenuResource);
            });

            Configure<AbpExceptionLocalizationOptions>(options =>
            {
                options.MapCodeNamespace("MenuSystem", typeof(MenuResource));
            });
        }
    }
}
