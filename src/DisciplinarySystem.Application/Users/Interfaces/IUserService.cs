using DisciplinarySystem.Application.Users.ViewModels.User;
using DisciplinarySystem.Domain.Users;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Users.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetListAsync(Expression<Func<User, bool>> filter = null, int skip = 0, int take = 10);
        Task<IEnumerable<SelectListItem>> GetSelectedUsersAsync();
        Task<List<SelectListItem>> GetSelectListRolesAsync();
        Task<User> GetById(Guid id);
        Task<IEnumerable<String>> GetUsersNameAsync();
        int GetCount(Expression<Func<User, bool>> filter = null);


        Task CreateAsync(CreateUser command);
        Task UpdateAsync(UpdateUser command);
    }
}
