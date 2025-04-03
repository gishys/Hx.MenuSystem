using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.MenuSystem.Application.Contracts
{
    public class CreateOrUpdateMenuUserDto
    {
        public required Guid[] MenuIds { get; set; }
        public required Guid UserId { get; set; }
    }
}
