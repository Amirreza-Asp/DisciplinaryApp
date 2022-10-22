using DisciplinarySystem.Application.FinalVotes.ViewModels;
using DisciplinarySystem.Domain.FinalVotes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.FinalVotes.Interfaces
{
    public interface IFinalVoteService
    {
        Task<IEnumerable<FinalVote>> ListAsync(Expression<Func<FinalVote, bool>> filter = null, int skip = 0, int take = 10);
        Task<FinalVote> GetByIdAsync(Guid id);
        int GetCount(Expression<Func<FinalVote, bool>> filter = null);
        Task<FinalVote> GetByCaseIdAsync(long caseId);
        Task<IEnumerable<SelectListItem>> GetSelectedVotesAsync();
        Task<FinalVoteDocument> GetDocumentByIdAsync(Guid id);

        Task CreateAsync(CreateFinalVote command, IFormFileCollection files);
        Task UpdateAsync(UpdateFinalVote command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id, long caseId);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
