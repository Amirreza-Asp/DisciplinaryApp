using DisciplinarySystem.Domain.PrimaryVotes;

namespace DisciplinarySystem.Presentation.Controllers.PrimaryVotes.ViewModels
{
	public class GetAllPrimaryVotes
	{
		public IEnumerable<PrimaryVote> PrimaryVotes { get; set; }
		public int TotalCount { get; set; }
		public PrimaryVoteFilter Filters { get; set; }
	}
}
