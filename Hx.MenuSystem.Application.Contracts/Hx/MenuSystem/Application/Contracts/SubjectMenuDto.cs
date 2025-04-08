using Hx.MenuSystem.Domain.Shared;

namespace Hx.MenuSystem.Application.Contracts
{
    public class SubjectMenuDto
    {
        public Guid SubjectId { get; set; }
        public Guid MenuId { get; set; }
        public double Order { get; set; }
        public SubjectType SubjectType { get; set; }
        public DateTime CreationTime { get; set; }
    }
}
