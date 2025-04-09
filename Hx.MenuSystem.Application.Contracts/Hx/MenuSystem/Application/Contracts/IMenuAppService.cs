using Hx.MenuSystem.Domain.Shared;

namespace Hx.MenuSystem.Application.Contracts
{
    public interface IMenuAppService
    {
        /// <summary>
        /// 获取菜单列表
        /// </summary>
        /// <param name="appName">应用名称</param>
        /// <param name="subjectId">赋予对象Id</param>
        /// <param name="refreshPermissions">刷新权限</param>
        /// <param name="type">赋予对象类型</param>
        /// <param name="displayName">显示名称</param>
        /// <param name="checkAuth">校验权限</param>
        /// <returns></returns>
        Task<List<MenuDto>> GetMenusByAppNameAsync(
            string appName,
            string subjectId,
            bool refreshPermissions = false,
            SubjectType type = SubjectType.User,
            string? displayName = null,
            bool checkAuth = true);
        Task<List<MenuDto>> GetCurrentUserMenusAsync(bool checkAuth = false);
        Task<MenuDto> CreateAsync(CreateOrUpdateMenuDto input);
        Task<List<MenuDto>> AddOrRemoveMenuUsersAsync(CreateOrUpdateMenuSubjectDto input);
        Task<MenuDto> UpdateAsync(Guid id, CreateOrUpdateMenuDto input);
        Task DeleteAsync(Guid id);
    }
}
