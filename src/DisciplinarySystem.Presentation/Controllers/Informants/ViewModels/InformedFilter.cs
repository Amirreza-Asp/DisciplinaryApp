namespace DisciplinarySystem.Presentation.Controllers.Informants.ViewModels
{
    public class InformedFilter
    {
        public String Subject { get; set; }
        public String FullName { get; set; }
        public DateTime CreateDate { get; set; }
        public long CaseId { get; set; }
        public bool OnlySee { get; set; }

        public int Skip { get; set; }
        public int Take { get; set; } = 10;


        public bool IsEmpty() => String.IsNullOrEmpty(Subject) && String.IsNullOrEmpty(FullName) && CreateDate == default;
    }
}
