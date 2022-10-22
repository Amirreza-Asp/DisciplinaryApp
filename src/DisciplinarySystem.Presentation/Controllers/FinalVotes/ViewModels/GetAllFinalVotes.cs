using DisciplinarySystem.Domain.FinalVotes;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes.ViewModels
{
    public class GetAllFinalVotes
    {
        public IEnumerable<FinalVote> FinalVotes { get; set; }
        public int TotalCount { get; set; }
        public FinalVoteFilter Filters { get; set; }
    }
}
