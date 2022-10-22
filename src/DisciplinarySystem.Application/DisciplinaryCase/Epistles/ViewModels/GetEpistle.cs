namespace DisciplinarySystem.Application.Epistles.ViewModels
{
    public class GetEpistle
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public String Type { get; set; }
        public String Subject { get; set; }
        public String Sender { get; set; }
        public String Reciver { get; set; }
        public long? ComplaintId { get; set; }
        public long? CaseId { get; set; }
    }
}
