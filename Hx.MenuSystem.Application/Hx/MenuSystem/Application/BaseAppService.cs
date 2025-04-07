using Hx.MenuSystem.Localization;
using Volo.Abp.Application.Services;

namespace Hx.MenuSystem.Application
{
    public class BaseAppService : ApplicationService
    {
        protected BaseAppService()
        {
            LocalizationResource = typeof(MenuResource);
        }
        public string GetLocalization(string name)
        {
            return L[name];
        }
    }
}
