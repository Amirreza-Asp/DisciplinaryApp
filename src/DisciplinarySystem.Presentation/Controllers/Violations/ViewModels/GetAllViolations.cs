using DisciplinarySystem.Application.Violations.ViewModels.Violation;

namespace DisciplinarySystem.Presentation.Controllers.Violations.ViewModels
{
    public class GetAllViolations
    {
        public List<GetViolatonDetails> Violations { get; set; }
        public int TotalCount { get; set; }
        public ViolationFilters Filters { get; set; }
    }
}
