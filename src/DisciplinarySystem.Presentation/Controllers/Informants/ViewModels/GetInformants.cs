using DisciplinarySystem.Application.Informants.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Informants.ViewModels
{
    public class GetInformants
    {
        public IEnumerable<InformedDetails> Informants { get; set; }
        public int TotalCount { get; set; }
        public InformedFilter Filters { get; set; }

    }
}
