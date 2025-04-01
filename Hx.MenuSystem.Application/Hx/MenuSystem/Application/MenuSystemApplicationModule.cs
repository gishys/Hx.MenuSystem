using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.AutoMapper;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Application
{
    [DependsOn(typeof(AbpAutoMapperModule))]
    [DependsOn(typeof(AbpDddApplicationModule))]
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
