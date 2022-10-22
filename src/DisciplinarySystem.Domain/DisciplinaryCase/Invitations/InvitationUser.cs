using DisciplinarySystem.Domain.Users;

namespace DisciplinarySystem.Domain.Invitations
{
    public class InvitationUser
    {
        public InvitationUser(Guid userId, Guid invitationId)
        {
            UserId = Guard.Against.Default(userId);
            InvitationId = Guard.Against.Default(invitationId);
        }

        public Guid UserId { get; private set; }
        public Guid InvitationId { get; private set; }

        public User User { get; private set; }
        public Invitation Invitation { get; set; }
    }
}
