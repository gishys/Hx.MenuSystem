using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.MenuSystem.Application.Contracts
{
    public class PermissionDto
    {
        public required string Name { get; set; }
        public required string Id { get; set; } // 权限来源 ID（用户 ID 或角色 ID）
    }
}
