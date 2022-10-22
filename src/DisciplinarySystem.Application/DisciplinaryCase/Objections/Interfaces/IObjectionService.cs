using DisciplinarySystem.Application.Objections.ViewModels;
using DisciplinarySystem.Domain.Objections;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Objections.Interfaces
{
    public interface IObjectionService
    {
        Task<IEnumerable<ObjectionDetails>> ListAsync(Expression<Func<Objection, bool>> filters = null, int skip = 0, int take = 10);
        Task<Objection> GetByIdAsync(Guid id);
        Task<IEnumerable<ObjectionDocument>> GetDocumentsAsync(Guid id);
        int GetCount(Expression<Func<Objection, bool>> filter = null);
        Task<ObjectionDocument> GetDocumentByIdAsync(Guid id);


        Task CreateAsync(CreateObjection command, IFormFileCollection files);
        Task UpdateAsync(UpdateObjection command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
