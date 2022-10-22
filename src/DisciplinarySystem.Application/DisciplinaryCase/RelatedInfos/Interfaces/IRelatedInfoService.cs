using DisciplinarySystem.Application.RelatedInfos.ViewModels;
using DisciplinarySystem.Domain.RelatedInfos;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.RelatedInfos.Interfaces
{
    public interface IRelatedInfoService
    {
        Task<IEnumerable<RelatedInfoDetails>> ListAsync(Expression<Func<RelatedInfo, bool>> filters = null, int skip = 0, int take = 10);
        Task<RelatedInfo> GetByIdAsync(Guid id);
        Task<IEnumerable<RelatedInfoDocument>> GetDocumentsAsync(Guid id);
        int GetCount(Expression<Func<RelatedInfo, bool>> filter = null);
        Task<RelatedInfoDocument> GetDocumentByIdAsync(Guid id);


        Task CreateAsync(CreateRelatedInfo command, IFormFileCollection files);
        Task UpdateAsync(UpdateRelatedInfo command, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id);
        Task<bool> RemoveFileAsync(Guid id);
    }
}
