using DisciplinarySystem.Application.Defences.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Defences.ViewModels
{
    public class GetAllDefences
    {
        public IEnumerable<DefenceDetails> Defences { get; set; }
        public int TotalCount { get; set; }
        public DefenceFilter Filters { get; set; }

    }
}
