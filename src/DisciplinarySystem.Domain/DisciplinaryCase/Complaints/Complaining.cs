using DisciplinarySystem.Domain.Invitations;

namespace DisciplinarySystem.Domain.Complaints
{
    public class Complaining : BaseEntity<Guid>
    {
        public Complaining(Guid id, string fullName, string studentNumber, NationalCode nationalCode,
            string? grade, string? educationalGroup, string? college, String? father)
        {
            Id = id;
            FullName = Guard.Against.NullOrEmpty(fullName);
            StudentNumber = Guard.Against.NullOrEmpty(studentNumber);
            NationalCode = Guard.Against.NullOrEmpty(nationalCode);
            Grade = grade;
            EducationalGroup = educationalGroup;
            College = college;
            Father = father;
        }

        private Complaining() { }

        public string FullName { get; private set; }
        public StudentNumber StudentNumber { get; private set; }
        public NationalCode NationalCode { get; private set; }
        public String? Grade { get; private set; }
        public String? EducationalGroup { get; private set; }
        public String? College { get; private set; }
        public String? Father { get; private set; }

        public Complaint Complaint { get; private set; }
        public ICollection<Invitation> Invitations { get; private set; }

        public Complaining WithFullName(String fullName)
        {
            FullName = Guard.Against.NullOrEmpty(fullName);
            return this;
        }
        public Complaining WithStudentNumber(StudentNumber studentNumber)
        {
            StudentNumber = studentNumber;
            return this;
        }
        public Complaining WithNationalCode(NationalCode nationalCode)
        {
            NationalCode = nationalCode;
            return this;
        }
        public Complaining WithGrade(String? grade)
        {
            Grade = grade;
            return this;
        }
        public Complaining WithEducationalGroup(String? educationalGroup)
        {
            EducationalGroup = educationalGroup;
            return this;
        }
        public Complaining WithCollege(String? college)
        {
            College = college;
            return this;
        }
        public Complaining WithFather(String? father)
        {
            Father = father;
            return this;
        }

    }
}
