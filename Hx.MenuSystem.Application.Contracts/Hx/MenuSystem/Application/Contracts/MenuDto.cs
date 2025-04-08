using Hx.MenuSystem.Domain.Shared;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Application.Dtos;

namespace Hx.MenuSystem.Application.Contracts
{
    public class MenuDto : EntityDto<Guid>
    {
        public Guid? TenantId { get; set; }

        [Required]
        [StringLength(MenuConsts.MaxNameLength)]
        public required string Name { get; set; }

        [StringLength(MenuConsts.MaxDisplayNameLength)]
        public required string DisplayName { get; set; }

        [StringLength(MenuConsts.MaxIconLength)]
        public string? Icon { get; set; }

        [StringLength(MenuConsts.MaxRouteLength)]
        public required string Route { get; set; }

        [StringLength(MenuConsts.MaxAppNameLength)]
        public required string AppName { get; set; }

        [StringLength(MenuConsts.MaxPermissionNameLength)]
        public required string PermissionName { get; set; }

        public Guid? AppFormId { get; set; }

        public Guid? ParentId { get; set; }
        public double Order { get; set; }
        public bool IsActive { get; set; }
        public bool IsGranted {  get; set; }
        public List<MenuDto> Children { get; set; } = [];
        public ICollection<SubjectMenuDto> Subjects { get; set; } = [];
    }
}
