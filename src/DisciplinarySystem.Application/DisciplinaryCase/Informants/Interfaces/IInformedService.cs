using DisciplinarySystem.Application.Informants.ViewModels;
using DisciplinarySystem.Domain.Informants;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Informants.Interfaces
{
    public interface IInformedService
    {
        Task<IEnumerable<InformedDetails>> GetListAsync(Expression<Func<Informed, bool>> filter = null, int skip = 0, int take = 10);
        Task<Informed> GetByIdAsync(Guid id);
        Task<List<InformedDocumentDto>> GetDocuments(Guid id);
        Task<InformedDocument> GetDocumentByIdAsync(Guid id);

        Task CreateAsync(CreateInformed command, IFormFileCollection files);
        Task UpdateAsync(UpdateInformed command, IFormFileCollection files);
        Task<bool> RemoveFileAsync(Guid id);
        Task<bool> RemoveAsync(Guid id);
        int GetCount(Expression<Func<Informed, bool>> filter = null);
    }
}
