using DisciplinarySystem.Application.Cases.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Cases.ViewModels
{
    public class GetCasesListVM
    {
        public List<GetCases> Cases { get; set; } = new List<GetCases>();
        public int TotalCount { get; set; }
        public CaseFilters Filters { get; set; }
    }
}
