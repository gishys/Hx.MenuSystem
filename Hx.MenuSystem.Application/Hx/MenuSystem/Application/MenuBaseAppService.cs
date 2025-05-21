using Hx.MenuSystem.Localization;
using Volo.Abp.Application.Services;

namespace Hx.MenuSystem.Application
{
    public class MenuBaseAppService : ApplicationService
    {
        protected MenuBaseAppService()
        {
            LocalizationResource = typeof(MenuResource);
        }
        public string GetLocalization(string name)
        {
            return L[name];
        }
    }
}
