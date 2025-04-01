using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace Hx.MenuSystem.Application.Contracts
{
    [DependsOn(typeof(AbpDddApplicationContractsModule))]
    public class MenuSystemApplicationContractModule : AbpModule
    {
    }
}
