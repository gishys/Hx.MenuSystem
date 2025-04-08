namespace Hx.MenuSystem.Application.Contracts
{
    public class CreateOrUpdateMenuSubjectDto
    {
        public bool IsGranted { get; set; }
        public required Guid[] MenuIds { get; set; }
        public required string SubjectId { get; set; }
        public required string SubjectType { get; set; }
    }
}
