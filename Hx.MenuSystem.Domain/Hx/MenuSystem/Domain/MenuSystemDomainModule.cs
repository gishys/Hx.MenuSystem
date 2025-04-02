using Hx.MenuSystem.Domain.Shared;
using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Domain
{
    [DependsOn(typeof(AbpDddDomainModule))]
    [DependsOn(typeof(MenuSystemDomainSharedModule))]
    public class MenuSystemDomainModule : AbpModule
    {
    }
}
