using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Domain.Epistles
{
    public class Epistle : BaseEntity<long>
    {
        public Epistle ( string type , string subject , string sender , string reciver , long? caseId , long? complaintId , string description )
        {
            Type = Guard.Against.NullOrEmpty(type);
            Subject = Guard.Against.NullOrEmpty(subject);
            Sender = Guard.Against.NullOrEmpty(sender);
            Reciver = Guard.Against.NullOrEmpty(reciver);
            Description = Guard.Against.NullOrEmpty(description);

            if ( caseId != null && caseId <= 0 )
                throw new ArgumentException("CaseId must have positive value");

            CaseId = caseId;

            if ( complaintId != null && complaintId <= 0 )
                throw new ArgumentException("CaseId must have positive value");

            ComplaintId = complaintId;
        }

        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public string Type { get; private set; }
        public string Subject { get; private set; }
        public string Sender { get; private set; }
        public string Reciver { get; private set; }
        public String Description { get; set; }
        public long? CaseId { get; private set; }
        public long? ComplaintId { get; set; }

        public Case Case { get; private set; }
        public Complaint Complaint { get; set; }
        public ICollection<EpistleDocument> Documents { get; private set; }



        public Epistle WithUpdateDate ( DateTime updateDate )
        {
            UpdateDate = updateDate;
            return this;
        }
        public Epistle WithSender ( String sender )
        {
            Sender = Guard.Against.NullOrEmpty(sender);
            return this;
        }
        public Epistle WithReciver ( String reciver )
        {
            Reciver = Guard.Against.NullOrEmpty(reciver);
            return this;
        }
        public Epistle WithType ( String type )
        {
            Type = Guard.Against.NullOrEmpty(type);
            return this;
        }
        public Epistle WithSubject ( String subject )
        {
            Subject = subject;
            return this;
        }
        public Epistle WithDescription ( String description )
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public Epistle WithCaseId ( long? caseId )
        {
            if ( caseId.HasValue && caseId.Value <= 0 )
                return this;

            CaseId = caseId;
            return this;
        }
        public Epistle WithComplaintId ( long? complaintId )
        {
            if ( complaintId.HasValue && complaintId.Value <= 0 )
                return this;

            ComplaintId = complaintId;
            return this;
        }
    }
}
