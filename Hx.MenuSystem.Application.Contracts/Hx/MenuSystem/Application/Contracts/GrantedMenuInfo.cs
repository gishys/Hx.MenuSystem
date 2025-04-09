namespace Hx.MenuSystem.Application.Contracts
{
    public class GrantedMenuInfo
    {
        public Guid MenuId { get; set; }
        public required string Type { get; set; }  // 权限类型，如 "U"（用户）或 "R"（角色）
        public required string Id { get; set; }    // 权限来源 ID（用户 ID 或角色 ID）
        public bool IsGranted {  get; set; }
    }
}
