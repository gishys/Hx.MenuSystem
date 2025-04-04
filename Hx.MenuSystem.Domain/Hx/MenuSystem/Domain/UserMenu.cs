﻿using Volo.Abp.Domain.Entities;

namespace Hx.MenuSystem.Domain
{
    // 用户-菜单关联实体
    public class UserMenu : Entity
    {
        public Guid UserId { get; set; }
        public Guid MenuId { get; set; }
        public double Order { get; set; }

        public UserMenu(Guid userId, Guid menuId, double order)
        {
            UserId = userId;
            MenuId = menuId;
            Order = order;
        }

        public override object[] GetKeys() => [UserId, MenuId];
    }
}
