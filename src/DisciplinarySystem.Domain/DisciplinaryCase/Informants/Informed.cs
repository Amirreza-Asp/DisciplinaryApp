namespace DisciplinarySystem.Domain.Informants
{
    public class Informed : BaseEntity<Guid>
    {
        public Informed(string fullName, PhoneNumber phoneNumber, string subject, string statements, long caseId, NationalCode nationalCode, string father)
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            FullName = Guard.Against.NullOrEmpty(fullName);
            PhoneNumber = phoneNumber;
            Subject = Guard.Against.NullOrEmpty(subject);
            Statements = Guard.Against.NullOrEmpty(statements);
            CaseId = Guard.Against.NegativeOrZero(caseId);
            NationalCode = nationalCode;
            Father = Guard.Against.NullOrEmpty(father);
        }

        private Informed() { }

        public String FullName { get; private set; }
        public PhoneNumber PhoneNumber { get; private set; }
        public NationalCode NationalCode { get; private set; }
        public String Father { get; set; }
        public String Subject { get; private set; }
        public String Statements { get; private set; }
        public long CaseId { get; private set; }
        public DateTime CreateDate { get; private set; }

        public Case Case { get; private set; }
        public ICollection<InformedDocument> Documents { get; set; }


        public Informed WithFullName(String fullName)
        {
            FullName = Guard.Against.NullOrEmpty(fullName);
            return this;
        }
        public Informed WithPhoneNumber(PhoneNumber phoneNumber)
        {
            PhoneNumber = phoneNumber;
            return this;
        }
        public Informed WithSubject(String subject)
        {
            Subject = Guard.Against.NullOrEmpty(subject);
            return this;
        }
        public Informed WithStatements(String statements)
        {
            Statements = Guard.Against.NullOrEmpty(statements);
            return this;
        }
        public Informed WithFather(String father)
        {
            Father = Guard.Against.NullOrEmpty(father);
            return this;
        }
        public Informed WithNationalCode(NationalCode nationalCode)
        {
            NationalCode = nationalCode;
            return this;
        }
    }
}
