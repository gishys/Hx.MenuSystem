using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Domain
{
    [DependsOn(typeof(AbpDddDomainModule))]
    public class MenuSystemDomainModule : AbpModule
    {
    }
}
