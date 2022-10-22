using DisciplinarySystem.Application.Epistles.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels
{
    public class GetEpistlesListVM
    {
        public IEnumerable<GetEpistle> Epistles { get; set; } = new List<GetEpistle>();
        public int TotalCount { get; set; }
        public EpistleFilter Filters { get; set; }
    }
}
