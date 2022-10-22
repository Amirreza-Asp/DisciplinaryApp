using DisciplinarySystem.Application.Common.Interfaces;
using DisciplinarySystem.Application.Violations.ViewModels.ViolationCategory;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Application.Violations.Intefaces
{
    public interface IViolationCategoryService : IServiceBase<ViolationCategory>
    {
        Task CreateAsync(CreateViolationCategory createViolationCategory);
        Task UpdateAsync(UpdateViolationCategory updateViolationCategory);
        Task<ViolationCategory> GetByTitleAsync(string title);
    }
}
