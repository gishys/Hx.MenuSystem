namespace Hx.MenuSystem.Application.Contracts
{
    public interface IMenuAppService
    {
        Task<List<MenuDto>> GetCurrentUserMenusAsync();
        Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input);
    }
}
