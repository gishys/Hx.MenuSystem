using Hx.MenuSystem.Domain.Shared;
using Volo.Abp.Domain.Entities;

namespace Hx.MenuSystem.Domain
{
    // 主体-菜单关联实体
    public class SubjectMenu(string subjectId, Guid menuId, double order, SubjectType subjectType, DateTime creationTime) : Entity
    {
        public virtual string SubjectId { get; protected set; } = subjectId;
        public virtual Guid MenuId { get; protected set; } = menuId;
        public virtual double Order { get; protected set; } = order;
        public virtual SubjectType SubjectType { get; protected set; } = subjectType;
        public virtual DateTime CreationTime { get; protected set; } = creationTime;

        public override object[] GetKeys() => [SubjectId, MenuId];
    }
}
