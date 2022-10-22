using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;

namespace DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes.ViewModels
{
	public class GetAllComitteeVotes
	{
		public IEnumerable<CentralCommitteeVote> CentralCommitteeVotes { get; set; }
		public CommitteeVoteFilter Filters { get; set; }
	}
}
