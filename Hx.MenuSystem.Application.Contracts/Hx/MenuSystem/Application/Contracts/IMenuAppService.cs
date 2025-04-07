namespace Hx.MenuSystem.Application.Contracts
{
    public interface IMenuAppService
    {
        Task<List<MenuDto>> GetCurrentUserMenusAsync(bool checkAuth = false);
        Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input);
        Task<List<MenuDto>> AddMenuUsersAsync(CreateOrUpdateMenuSubjectDto input);
        Task PutMenuUsersAsync(CreateOrUpdateMenuSubjectDto input);
        Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input);
        Task DeleteAsync(Guid id);
    }
}
