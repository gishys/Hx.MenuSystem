﻿using Hx.MenuSystem.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using Volo.Abp;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace Hx.MenuSystem.Domain
{
    public class Menu : AggregateRoot<Guid>, IMultiTenant
    {
        public virtual Guid? TenantId { get; set; }

        [Required]
        [StringLength(MenuConsts.MaxNameLength)]
        public virtual string Name { get; private set; }

        [StringLength(MenuConsts.MaxDisplayNameLength)]
        public virtual string DisplayName { get; private set; }

        [StringLength(MenuConsts.MaxIconLength)]
        public virtual string? Icon { get; private set; }

        [StringLength(MenuConsts.MaxRouteLength)]
        public string Route { get; private set; }

        [StringLength(MenuConsts.MaxAppNameLength)]
        public virtual string AppName { get; private set; }

        [StringLength(MenuConsts.MaxPermissionNameLength)]
        public virtual string PermissionName { get; private set; }

        public virtual Guid? AppFormId { get; private set; }

        public virtual Guid? ParentId { get; private set; }
        public virtual double Order { get; set; }
        public virtual bool IsActive { get; set; } = true;

        // 导航属性
        public virtual ICollection<UserMenu> Users { get; set; } = new List<UserMenu>();

        private Menu() { }

        public Menu(
            Guid id,
            string name,
            string displayName,
            string route,
            string appName,
            string permissionName,
            string? icon,
            double order,
            bool isActive,
            Guid? parentId = null,
            Guid? appFormId = null)
        {
            SetName(name);
            SetDisplayName(displayName);
            SetRoute(route);
            SetAppName(appName);
            Icon = icon;
            SetPermissionName(permissionName);
            AppFormId = appFormId;
            ParentId = parentId;
            Id = id;
            IsActive = isActive;
            Order = order;
        }

        public void SetName(string name)
        {
            Name = Check.NotNullOrWhiteSpace(name, nameof(name));
        }
        public void SetRoute(string route)
        {
            Route = Check.NotNullOrWhiteSpace(route, nameof(route));
        }
        public void SetAppName(string appName)
        {
            AppName = Check.NotNullOrWhiteSpace(appName, nameof(appName));
        }
        public void SetPermissionName(string permissionName)
        {
            PermissionName = Check.NotNullOrWhiteSpace(permissionName, nameof(permissionName));
        }
        public void SetIcon(string? icon)
        {
            Icon = icon;
        }
        public void SetDisplayName(string displayName)
        {
            DisplayName = Check.NotNullOrWhiteSpace(displayName, nameof(displayName));
        }
        public void SetOrder(double order)
        {
            Order = order;
        }
        public void SetAppFormId(Guid? appFormId)
        {
            AppFormId = appFormId;
        }
        public void SetParentId(Guid? parentId)
        {
            ParentId = parentId;
        }
        public void SetIsActive(bool isActive)
        {
            IsActive = isActive;
        }
        public void AddOrUpdateUser(Guid userId)
        {
            if (Users.Any(u => u.UserId == userId)) return;
            double maxOrder = Users.Count > 0 ? Users.Max(u => u.Order) : 0;
            double newOrder = maxOrder + 1;
            Users.Add(new UserMenu(
                userId: userId,
                menuId: Id,
                order: newOrder
            ));
        }
    }
}