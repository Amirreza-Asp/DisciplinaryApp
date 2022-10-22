namespace DisciplinarySystem.Presentation.Controllers.RelatedInfos.ViewModels
{
    public class RelatedInfoFilter
    {
        public long CaseId { get; set; }

        public String Subject { get; set; }
        public DateTime CreateDate { get; set; }
        public bool OnlySee { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 10;


        public bool IsEmpty() => String.IsNullOrEmpty(Subject) && CreateDate == default;
    }
}
