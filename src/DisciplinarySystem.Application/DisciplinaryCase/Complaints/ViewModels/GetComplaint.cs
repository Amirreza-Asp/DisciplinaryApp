namespace DisciplinarySystem.Application.Complaints.ViewModels
{
    public class GetComplaint
    {
        public long Id { get; set; }
        public String Subject { get; set; }
        public String ComplainingName { get; set; }
        public String PlaintiffName { get; set; }
        public DateTime CreateTime { get; set; }
        public String Status { get; set; }
        public String Result { get; set; }
    }
}
