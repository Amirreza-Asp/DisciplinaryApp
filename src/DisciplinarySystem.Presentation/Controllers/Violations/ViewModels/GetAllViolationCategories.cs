using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Presentation.Controllers.Violations.ViewModels
{
    public class GetAllViolationCategories
    {
        public List<ViolationCategory> ViolationCategories { get; set; }
        public int TotalCount { get; set; }
        public ViolationCategoryFilter Filters { get; set; }
    }
}
