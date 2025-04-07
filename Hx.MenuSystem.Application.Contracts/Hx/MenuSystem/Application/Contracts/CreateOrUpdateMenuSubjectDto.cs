using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.MenuSystem.Application.Contracts
{
    public class CreateOrUpdateMenuSubjectDto
    {
        public required Guid[] MenuIds { get; set; }
        public required Guid SubjectId { get; set; }
        public required string SubjectType { get; set; }
    }
}
