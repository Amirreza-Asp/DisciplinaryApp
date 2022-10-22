using DisciplinarySystem.Application.Objections.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Objections.ViewModels
{
    public class GetAllObjections
    {
        public IEnumerable<ObjectionDetails> Objections { get; set; }
        public int TotalCount { get; set; }
        public ObjectionFilter Filters { get; set; }
    }
}
