namespace DisciplinarySystem.Domain.Defences
{
    public class Defence : BaseEntity<Guid>
    {
        public Defence(string subject, string description, long caseId)
        {
            Id = Guid.NewGuid();
            Subject = subject;
            Description = description;
            CaseId = caseId;
        }

        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public String Subject { get; private set; }
        public String Description { get; private set; }
        public long CaseId { get; private set; }

        public Case Case { get; private set; }
        public ICollection<DefenceDocument> Documents { get; private set; }

        public Defence WithUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
            return this;
        }
        public Defence WithSubject(String subject)
        {
            Subject = Guard.Against.NullOrEmpty(subject);
            return this;
        }
        public Defence WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
    }
}
