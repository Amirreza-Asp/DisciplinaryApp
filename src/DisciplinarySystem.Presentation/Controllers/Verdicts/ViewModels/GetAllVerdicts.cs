using DisciplinarySystem.Domain.Verdicts;

namespace DisciplinarySystem.Presentation.Controllers.Verdicts.ViewModels
{
    public class GetAllVerdicts
    {
        public IEnumerable<Verdict> Verdicts { get; set; }
        public int TotalCount { get; set; }
        public VerdictFilter Filters { get; set; }
    }
}
