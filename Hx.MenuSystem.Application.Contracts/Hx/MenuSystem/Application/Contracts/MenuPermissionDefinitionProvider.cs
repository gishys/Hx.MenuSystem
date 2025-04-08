using Hx.MenuSystem.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Hx.MenuSystem.Application.Contracts
{
    internal class MenuPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup("MenuSystem", L("Permission:MenuSystem"));
            var menuPermission = myGroup.AddPermission("MenuSystem.List", L("Permission:MenuSystem.List"));
            menuPermission.AddChild("MenuSystem.MenuManagement", L("Permission:MenuSystem.MenuManagement"));
            menuPermission.AddChild("MenuSystem.GrantedAuth", L("Permission:MenuSystem.GrantedAuth"));
        }
        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<MenuResource>(name);
        }
    }
}
