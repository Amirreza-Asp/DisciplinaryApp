using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.User;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel;
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
        private readonly IPasswordHasher _passwordHasher;

        public UserService ( IRepository<User> userRepo , IRepository<Role> roleRepo , IUserApi userApi , IRepository<AuthUser> authUserRepo , IPasswordHasher passwordHasher )
        {
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _userApi = userApi;
            _authUserRepo = authUserRepo;
            _passwordHasher = passwordHasher;
        }

        public async Task CreateAsync ( CreateUser command )
        {
            var user = new User(command.FullName , command.NationalCode.ToString() ,
                command.StartDate , command.EndDate , command.RoleId , command.Type);

            var userApi = await _userApi.GetUserAsync(user.NationalCode);

            if ( String.IsNullOrEmpty(userApi.Mobile) )
                userApi.Mobile = SD.DefaultPhoneNumber;

            _authUserRepo.Add(new AuthUser(userApi.Mobile , userApi.Idmelli , userApi.Name
                , userApi.Lastname , userApi.Idmelli , _passwordHasher.HashPassword(userApi.Idmelli) , command.Access , user.Id));

            _userRepo.Add(user);
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

        public async Task<IEnumerable<string>> GetUsersNameAsync ( String type )
        {
            return await _userRepo.GetAllAsync(filter: u => u.AttendenceTime.To.Date > DateTime.Now.Date && u.Type == type , select: u => u.FullName);
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
                .WithRoleId(command.RoleId)
                .WithType(command.Type);

            AuthUser authUser = await _authUserRepo.FirstOrDefaultAsync(u => u.UserId == user.Id);
            if ( authUser != null )
            {
                authUser.WithRoleId(command.Access);
                _authUserRepo.Update(authUser);
            }

            _userRepo.Update(user);
            await _userRepo.SaveAsync();
        }
    }
}
