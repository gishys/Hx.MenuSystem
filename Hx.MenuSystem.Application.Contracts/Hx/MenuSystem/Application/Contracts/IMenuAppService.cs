namespace Hx.MenuSystem.Application.Contracts
{
    public interface IMenuAppService
    {
        Task<List<MenuDto>> GetCurrentUserMenusAsync();
        Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input);
        Task<MenuDto> AddMenuUsersAsync(CreateOrUpdateMenuUserDto input);
        Task<MenuDto> RemoveMenuUsersAsync(Guid menuId, Guid userId);
        Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input);
    }
}
