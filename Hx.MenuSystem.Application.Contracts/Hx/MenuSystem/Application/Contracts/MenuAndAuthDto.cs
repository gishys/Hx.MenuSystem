using Volo.Abp.PermissionManagement;

namespace Hx.MenuSystem.Application.Contracts
{
    public class MenuAndAuthDto
    {
        public required List<MenuDto> Menus { get; set; }
        public required List<PermissionGrantInfoDto> Auths { get; set; }
    }
}
