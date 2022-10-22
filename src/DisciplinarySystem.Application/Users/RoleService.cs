using DisciplinarySystem.Application.Common;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.Role;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.Application.Users
{
    public class RoleService : ServiceBase<Role>, IRoleService
    {
        private IRepository<Role> _roleRepo;

        public RoleService(IRepository<Role> roleRepo) : base(roleRepo)
        {
            _roleRepo = roleRepo;
        }

        public async Task CreateAsync(CreateRole createRole)
        {
            var role = new Role(createRole.Title, createRole.Description);

            _roleRepo.Add(role);
            await _roleRepo.SaveAsync();
        }

        public async Task<Role> GetByTitleAsync(String title)
        {
            return await _roleRepo.FirstOrDefaultAsync(u => u.Title == title);
        }

        public async Task UpdateAsync(UpdateRole updateRole)
        {
            var role = new Role(updateRole.Id, updateRole.Title, updateRole.Description);
            _roleRepo.Update(role);
            await _roleRepo.SaveAsync();
        }
    }
}
