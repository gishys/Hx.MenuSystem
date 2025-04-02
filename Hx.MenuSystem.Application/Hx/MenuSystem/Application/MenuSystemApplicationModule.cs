using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;
using Hx.MenuSystem.Domain;
using Hx.MenuSystem.EntityFrameworkCore;
using Hx.MenuSystem.Application.Contracts;

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

            Configure<AbpAutoMapperOptions>(options =>
            {
                options.AddProfile<MenuSystemProfile>(validate: true);
            });
        }
    }
}
