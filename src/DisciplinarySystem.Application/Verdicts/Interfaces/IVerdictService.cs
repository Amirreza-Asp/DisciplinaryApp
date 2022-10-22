using DisciplinarySystem.Application.Verdicts.ViewModels;
using DisciplinarySystem.Domain.Verdicts;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Verdicts.Interfaces
{
    public interface IVerdictService
    {
        Task<Verdict> GetByIdAsync(long id);
        Task<List<Verdict>> GetListAsync(int skip = 0, int take = 10);
        Task<IEnumerable<SelectListItem>> GetSelectListAsync();
        int GetCount(Expression<Func<Verdict, bool>> filter = null);
        Task<Verdict> GetByTitleAsync(string title);
        Task<IEnumerable<VotesNumberForFinalVote>> GetFinalVoteCountAsync();

        Task<bool> RemoveAsync(long id);
        Task CreateAsync(CreateVerdict command);
        Task UpdateAsync(UpdateVerdict command);
    }
}
