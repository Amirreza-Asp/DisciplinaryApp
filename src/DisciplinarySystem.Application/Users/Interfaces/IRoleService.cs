using DisciplinarySystem.Application.Common.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.Role;
using DisciplinarySystem.Domain.Users;

namespace DisciplinarySystem.Application.Users.Interfaces
{
    public interface IRoleService : IServiceBase<Role>
    {
        Task CreateAsync(CreateRole command);
        Task UpdateAsync(UpdateRole command);
        Task<Role> GetByTitleAsync(string title);
    }
}
