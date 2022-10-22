using DisciplinarySystem.Application.Invitations.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Invitations.ViewModels
{
    public class GetAllInvitations
    {
        public IEnumerable<InvitationDetails> Invitations { get; set; }
        public int TotalCount { get; set; }
        public InvitationFilter Filters { get; set; }
    }
}
