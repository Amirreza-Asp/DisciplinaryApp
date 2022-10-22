using DisciplinarySystem.Application.PrimaryVotes.ViewModels;
using DisciplinarySystem.Domain.PrimaryVotes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.PrimaryVotes.Interfaces
{
	public interface IPrimaryVoteService
	{
		Task<IEnumerable<PrimaryVote>> ListAsync(Expression<Func<PrimaryVote, bool>> filter = null, int skip = 0, int take = 10);
		Task<PrimaryVote> GetByIdAsync(Guid id);
		int GetCount(Expression<Func<PrimaryVote, bool>> filter = null);
		Task<IEnumerable<SelectListItem>> GetSelectedVotesAsync();
		Task<PrimaryVote> GetByCaseIdAsync(long caseId);
		Task<PrimaryVoteDocument> GetDocumentByIdAsync(Guid id);

		Task CreateAsync(CreatePrimaryVote command, IFormFileCollection files);
		Task UpdateAsync(UpdatePrimaryVote command, IFormFileCollection files);
		Task<bool> RemoveAsync(Guid id, long caseId);
		Task<bool> RemoveFileAsync(Guid id);
	}
}
