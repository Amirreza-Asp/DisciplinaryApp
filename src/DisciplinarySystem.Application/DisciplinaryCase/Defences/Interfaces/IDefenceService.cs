using DisciplinarySystem.Application.Defences.ViewModels;
using DisciplinarySystem.Domain.Defences;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Defences.Interfaces
{
    public interface IDefenceService
    {
        Task<IEnumerable<DefenceDetails>> ListAsync(Expression<Func<Defence, bool>> filters = null, int skip = 0, int take = 10);
        Task<Defence> GetByIdWithCaseAsync(Guid id);
        Task<Defence> GetByIdAsync(Guid id);
        Task<IEnumerable<DefenceDocument>> GetDocumentsAsync(Guid id);
        int GetCount(Expression<Func<Defence, bool>> filter = null);
        Task<DefenceDocument> GetDocumentByIdAsync(Guid id);

        Task CreateAsync(CreateDefence command, IFormFileCollection files);
        Task UpdateAsync(UpdateDefence command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
