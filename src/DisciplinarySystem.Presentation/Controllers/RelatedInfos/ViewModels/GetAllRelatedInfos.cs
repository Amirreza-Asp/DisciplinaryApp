using DisciplinarySystem.Application.RelatedInfos.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.RelatedInfos.ViewModels
{
    public class GetAllRelatedInfos
    {
        public IEnumerable<RelatedInfoDetails> RelatedInfos { get; set; }
        public int TotalCount { get; set; }
        public RelatedInfoFilter Filters { get; set; }
    }
}
