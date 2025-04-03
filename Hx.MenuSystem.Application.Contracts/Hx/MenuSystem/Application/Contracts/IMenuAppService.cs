namespace Hx.MenuSystem.Application.Contracts
{
    public interface IMenuAppService
    {
        Task<List<MenuDto>> GetCurrentUserMenusAsync(bool checkAuth = false);
        Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input);
        Task<List<MenuDto>> AddMenuUsersAsync(CreateOrUpdateMenuUserDto input);
        Task RemoveMenuUsersAsync(Guid[] menuIds, Guid userId);
        Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input);
        Task DeleteAsync(Guid id);
    }
}
