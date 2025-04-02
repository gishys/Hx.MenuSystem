using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.MenuSystem.Application.Contracts
{
    public class CreateOrUpdateMenuUserDto
    {
        public Guid MenuId { get; set; }
        public Guid UserId { get; set; }
        public double Order {  get; set; }
    }
}
