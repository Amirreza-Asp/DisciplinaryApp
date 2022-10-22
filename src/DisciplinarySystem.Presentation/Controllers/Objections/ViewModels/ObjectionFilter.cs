namespace DisciplinarySystem.Presentation.Controllers.Objections.ViewModels
{
    public class ObjectionFilter
    {
        public long CaseId { get; set; }

        public String Subject { get; set; }
        public String Result { get; set; }
        public DateTime CreateDate { get; set; }
        public bool OnlySee { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 10;


        public bool IsEmpty() => String.IsNullOrEmpty(Subject) && String.IsNullOrEmpty(Result) && CreateDate == default;
    }
}
