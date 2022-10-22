namespace DisciplinarySystem.Application.Invitations.ViewModels
{
    public class InvitationDetails
    {
        public Guid Id { get; set; }
        public String Subject { get; set; }
        public DateTime InviteDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
    }
}
