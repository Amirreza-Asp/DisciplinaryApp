namespace DisciplinarySystem.Domain.RelatedInfos
{
    public class RelatedInfo : BaseEntity<Guid>
    {
        public RelatedInfo(string subject, string description, long caseId)
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            Subject = subject;
            Description = description;
            CaseId = caseId;
        }

        public DateTime CreateDate { get; private set; }
        public String Subject { get; private set; }
        public String Description { get; private set; }
        public long CaseId { get; private set; }

        public Case Case { get; private set; }
        public ICollection<RelatedInfoDocument> Documents { get; private set; }

        public RelatedInfo WithSubject(String subject)
        {
            Subject = Guard.Against.NullOrEmpty(subject);
            return this;
        }
        public RelatedInfo WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
    }
}
