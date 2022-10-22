using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Domain.Invitations
{
    public class Invitation : BaseEntity<Guid>
    {
        public Invitation(string subject, string description, long caseId, DateTime inviteDate, Guid? complainingId, Guid? plaintiffId)
        {
            Id = Guid.NewGuid();
            Subject = Guard.Against.NullOrEmpty(subject);
            Description = Guard.Against.NullOrEmpty(description);
            CaseId = Guard.Against.NegativeOrZero(caseId);
            InviteDate = Guard.Against.Default(inviteDate);
            ComplainingId = complainingId;
            PlaintiffId = plaintiffId;
        }

        public String Subject { get; private set; }
        public String Description { get; private set; }
        public long CaseId { get; private set; }
        public Guid? ComplainingId { get; private set; }
        public Guid? PlaintiffId { get; private set; }
        public DateTime? UpdateDate { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime InviteDate { get; set; }

        public Complaining Complaining { get; private set; }
        public Plaintiff Plaintiff { get; private set; }
        public Case Case { get; private set; }
        public ICollection<InvitationUser> InvitationUsers { get; private set; }
        public ICollection<InvitationDocument> Documents { get; private set; }


        public Invitation WithUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
            return this;
        }
        public Invitation WithSubject(String subject)
        {
            Subject = Guard.Against.NullOrEmpty(subject);
            return this;
        }
        public Invitation WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public Invitation WithComplainingId(Guid? complainingId)
        {
            ComplainingId = complainingId;
            return this;
        }
        public Invitation WithPlaintiffId(Guid? plaitiffId)
        {
            PlaintiffId = plaitiffId;
            return this;
        }
        public Invitation WithInviteDate(DateTime inviteDate)
        {
            InviteDate = Guard.Against.Default(inviteDate);
            return this;
        }

    }
}
