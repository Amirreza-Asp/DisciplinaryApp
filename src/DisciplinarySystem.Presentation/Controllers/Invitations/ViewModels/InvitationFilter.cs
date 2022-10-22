namespace DisciplinarySystem.Presentation.Controllers.Invitations.ViewModels
{
    public class InvitationFilter
    {
        public String Subject { get; set; }
        public DateTime CreateDate { get; set; }
        public bool OnlySee { get; set; }

        public long CaseId { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 10;

        public bool IsEmpty() => String.IsNullOrEmpty(Subject) && CreateDate == default;
    }
}
