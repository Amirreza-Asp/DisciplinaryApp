namespace DisciplinarySystem.Application.Violations.ViewModels.Violation
{
    public class GetViolatonDetails
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String Category { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public String? Vote { get; set; }
    }
}
