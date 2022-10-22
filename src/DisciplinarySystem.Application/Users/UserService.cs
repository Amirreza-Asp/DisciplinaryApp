using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.User;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Authentication.Interfaces;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DisciplinarySystem.Application.Users
{
    public class UserService : IUserService
    {
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;
        private readonly IUserApi _userApi;
        private readonly IRepository<AuthUser> _authUserRepo;
        private readonly IUserRoleRepository _userRoleRepo;
        private readonly IPasswordHasher _passwordHasher;

        public UserService ( IRepository<User> userRepo , IRepository<Role> roleRepo , IUserApi userApi , IRepository<AuthUser> authUserRepo , IUserRoleRepository userRoleRepo , IPasswordHasher passwordHasher )
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userApi = userApi;
            _authUserRepo = authUserRepo;
            _userRoleRepo = userRoleRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateAsync ( CreateUser command )
        {
            var user = new User(command.FullName , command.NationalCode.ToString() ,
                command.StartDate , command.EndDate , command.RoleId);

            var userApi = await _userApi.GetUserAsync(user.NationalCode);


            _authUserRepo.Add(new AuthUser(userApi.Mobile , userApi.Idmelli , userApi.Name , userApi.Lastname , userApi.Idmelli , _passwordHasher.HashPassword(userApi.Idmelli)));

            _userRepo.Add(user);
            await _userRepo.SaveAsync();

            var authUser = await _authUserRepo.FirstOrDefaultAsync(u => u.UserName == userApi.Idmelli);
            _userRoleRepo.Add(new UserRole(authUser.Id , command.Access));

            await _userRepo.SaveAsync();
        }

        public async Task<IEnumerable<SelectListItem>> GetSelectedUsersAsync ()
        {
            return await _userRepo.GetAllAsync<SelectListItem>(
                        filter: u => u.AttendenceTime.To.Date > DateTime.Now.Date ,
                        select: user => new SelectListItem
                        {
                            Text = user.FullName ,
                            Value = user.Id.ToString()
                        }
                   );
        }

        public async Task<User> GetById ( Guid id )
        {
            return await _userRepo.FirstOrDefaultAsync(
                filter: u => u.Id == id ,
                include: source => source.Include(u => u.Role));
        }

        public int GetCount ( Expression<Func<User , bool>> filter = null ) => _userRepo.GetCount(filter);

        public async Task<IEnumerable<User>> GetListAsync ( Expression<Func<User , bool>> filter = null , int skip = 0 , int take = 10 )
        {
            return await _userRepo.GetAllAsync(
                filter ,
                orderBy: source => source
                    .OrderByDescending(u => u.AttendenceTime.To)
                    .OrderByDescending(u => u.AttendenceTime.From) ,
                include: source => source.Include(u => u.Role) ,
                skip: skip ,
                take: take);
        }

        public async Task<List<SelectListItem>> GetSelectListRolesAsync ()
        {
            var roles = await _roleRepo.GetAllAsync();
            return roles.Select(role => new SelectListItem
            {
                Text = role.Title ,
                Value = role.Id.ToString()
            }).ToList();
        }

        public async Task<IEnumerable<string>> GetUsersNameAsync ()
        {
            return await _userRepo.GetAllAsync(u => u.AttendenceTime.To.Date > DateTime.Now.Date , select: u => u.FullName);
        }

        public async Task UpdateAsync ( UpdateUser command )
        {
            User user = await _userRepo.FindAsync(command.Id);
            if ( user == null )
                return;

            user.WithFullName(command.FullName)
                .WithNationalCode(command.NationalCode)
                .WithStartDate(command.StartDate)
                .WithEndDate(command.EndDate)
                .WithRoleId(command.RoleId);

            _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }
    }
}
