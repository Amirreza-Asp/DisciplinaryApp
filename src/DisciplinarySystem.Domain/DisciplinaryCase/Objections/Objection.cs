namespace DisciplinarySystem.Domain.Objections
{
    public class Objection : BaseEntity<Guid>
    {
        public Objection(string subject, string description, string result, long caseId)
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            Subject = Guard.Against.NullOrEmpty(subject);
            Description = Guard.Against.NullOrEmpty(description);
            Result = Guard.Against.NullOrEmpty(result);
            CaseId = Guard.Against.NegativeOrZero(caseId);
        }

        public String Subject { get; private set; }
        public String Description { get; private set; }
        public DateTime CreateDate { get; set; }
        public String Result { get; private set; }

        public long CaseId { get; private set; }
        public Case Case { get; private set; }
        public ICollection<ObjectionDocument> Documents { get; private set; }



        public Objection WithSubject(String subject)
        {
            Subject = Guard.Against.NullOrEmpty(subject);
            return this;
        }
        public Objection WithResult(String result)
        {
            Result = Guard.Against.NullOrEmpty(result);
            return this;
        }
        public Objection WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }

    }
}
