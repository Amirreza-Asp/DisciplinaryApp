using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases;
using DisciplinarySystem.Domain.Epistles;

namespace DisciplinarySystem.Domain.Complaints
{
    public class Complaint : BaseEntity<long>
    {
        public Complaint(string title, Guid plaintiffId, Guid complainingId, string description)
        {
            Title = Guard.Against.NullOrEmpty(title);
            PlaintiffId = Guard.Against.Default(plaintiffId);
            ComplainingId = Guard.Against.Default(complainingId);
            Description = Guard.Against.NullOrEmpty(description);
            CreateDate = DateTime.Now;
            Result = ComplaintResult.Archive;
        }

        private Complaint() { }

        public String Title { get; private set; }
        public ComplaintResult Result { get; private set; }
        public DateTime CreateDate { get; private set; }
        public String Description { get; private set; }
        public Guid ComplainingId { get; private set; }
        public Guid PlaintiffId { get; private set; }

        public Complaining Complaining { get; private set; }
        public Plaintiff Plaintiff { get; private set; }
        public List<ComplaintDocument> Documents { get; private set; }
        public Case Case { get; private set; }
        public ICollection<Epistle> Epistles { get; set; }


        public Complaint WithTitle(String title)
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Complaint WithResult(ComplaintResult result)
        {
            Result = result;
            return this;
        }

        public Complaint WithDescription(String descriptions)
        {
            Description = Guard.Against.NullOrEmpty(descriptions);
            return this;
        }



    }
}
