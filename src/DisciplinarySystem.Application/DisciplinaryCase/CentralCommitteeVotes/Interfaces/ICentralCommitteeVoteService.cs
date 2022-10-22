using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.ViewModels;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.Interfaces
{
    public interface ICentralCommitteeVoteService
    {

        Task<IEnumerable<CentralCommitteeVote>> ListAsync(Expression<Func<CentralCommitteeVote, bool>> filter = null, int skip = 0, int take = 10);
        Task<CentralCommitteeVote> GetByIdAsync(Guid id);
        int GetCount(Expression<Func<CentralCommitteeVote, bool>> filter = null);
        Task<CentralCommitteeVoteDocument> GetDocumentByIdAsync(Guid id);
        Task<CentralCommitteeVote> GetByCaseIdAsync(long id);

        Task CreateAsync(CreateCentralCommitteeVote command, IFormFileCollection files);
        Task UpdateAsync(UpdateCentralCommitteeVote command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id, long caseId);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
