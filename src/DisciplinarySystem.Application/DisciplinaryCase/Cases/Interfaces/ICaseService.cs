using DisciplinarySystem.Application.Cases.ViewModels;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Cases.Interfaces
{
    public interface ICaseService
    {
        Task<IEnumerable<GetCases>> GetAllAsync(Expression<Func<Case, bool>> filter = null, int skip = 0, int take = 10);
        Task<Case> GetByComplaintIdAsync(long id);
        Task<Case> GetByIdAsync(long id);
        Task<Case> FullInformationAsync(long id);
        long GetCount(Expression<Func<Case, bool>> filter = null);

        Task CreateAsync(CreateCase command);
        Task<bool> RemoveAsync(long id);
    }
}
