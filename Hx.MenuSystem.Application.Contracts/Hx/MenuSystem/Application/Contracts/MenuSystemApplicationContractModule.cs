using Hx.MenuSystem.Domain.Shared;
using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Application.Contracts
{
    [DependsOn(typeof(MenuSystemDomainSharedModule))]
    [DependsOn(typeof(AbpDddApplicationContractsModule))]
    public class MenuSystemApplicationContractModule : AbpModule
    {
    }
}
