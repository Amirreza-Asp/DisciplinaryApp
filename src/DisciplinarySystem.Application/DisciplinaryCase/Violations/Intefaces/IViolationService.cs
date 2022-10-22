using DisciplinarySystem.Application.Violations.ViewModels.Violation;
using DisciplinarySystem.Domain.Violations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Violations.Intefaces
{
    public interface IViolationService
    {
        Task<List<GetViolatonDetails>> GetAllAsync(Expression<Func<Violation, bool>> filter = null, int skip = 0, int take = 10);
        Task<IEnumerable<SelectListItem>> GetSelectedListAsync();
        Task<Violation> GetByIdAsync(Guid id);
        Task<Violation> FindAsync(
            Expression<Func<Violation,bool>> filter ,
            Func<IQueryable<Violation>, IIncludableQueryable<Violation, object>> include = null);
        int GetCount(Expression<Func<Violation, bool>> filter = null);
        Task<ViolationDocument> GetDocumentByIdAsync(Guid id);

        Task CreateAsync(CreateViolation createViolation, IFormFileCollection files);
        Task<bool> RemoveAsync(Guid id);
        Task UpdateAsync(UpdateViolation updateViolation, IFormFileCollection files);

        Task<bool> RemoveDocument(Guid id);
    }
}
