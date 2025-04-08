using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.MenuSystem.Application.Contracts
{
    // 辅助类，表示权限来源
    public class GrantedSource
    {
        public required string Type { get; set; }
        public required string Id { get; set; }
    }
}
