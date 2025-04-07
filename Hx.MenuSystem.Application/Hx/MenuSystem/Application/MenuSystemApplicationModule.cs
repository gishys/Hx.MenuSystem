using Hx.MenuSystem.Application.Contracts;
using Hx.MenuSystem.Domain;
using Hx.MenuSystem.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Volo.Abp.PermissionManagement;

namespace Hx.MenuSystem.Application
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
    [DependsOn(typeof(MenuSystemDomainModule))]
    [DependsOn(typeof(MenuSystemApplicationContractModule))]
    [DependsOn(typeof(MenuSystemEntityFrameworkCoreModule))]
    public class MenuSystemApplicationModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            context.Services.AddAutoMapperObjectMapper<MenuSystemApplicationModule>();
            context.Services.AddTransient<IPermissionAppService, HxPermissionAppService>();
            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MenuSystemProfile>(validate: true);
            });
        }
    }
}
