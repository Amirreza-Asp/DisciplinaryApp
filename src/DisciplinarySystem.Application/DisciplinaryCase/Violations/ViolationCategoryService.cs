using DisciplinarySystem.Application.Common;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Application.Violations.ViewModels.ViolationCategory;
using DisciplinarySystem.Domain.Violations;
using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.Application.Violations
{
    public class ViolationCategoryService : ServiceBase<ViolationCategory>, IViolationCategoryService
    {
        private readonly IRepository<ViolationCategory> _repo;

        public ViolationCategoryService(IRepository<ViolationCategory> repo) : base(repo)
        {
            _repo = repo;
        }

        public async Task CreateAsync(CreateViolationCategory createViolationCategory)
        {
            var entity = new ViolationCategory(createViolationCategory.Title, createViolationCategory.Description);

            _repo.Add(entity);
            await _repo.SaveAsync();
        }

        public async Task<ViolationCategory?> GetByTitleAsync(String title)
        {
            var role = await _repo.FirstOrDefaultAsync(u => u.Title == title);
            return role;
        }

        public async Task UpdateAsync(UpdateViolationCategory updateRole)
        {
            var entity = new ViolationCategory(updateRole.Id, updateRole.Title, updateRole.Description);
            _repo.Update(entity);
            await _repo.SaveAsync();
        }
    }
}
