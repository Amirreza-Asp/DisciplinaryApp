using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Persistence.Data.Initializer.Interfaces;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.EntityFrameworkCore;

namespace DisciplinarySystem.Persistence.Data.Initializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext _db;
        private readonly IRepository<AuthRole> _roleManager;
        private readonly IRepository<AuthUser> _userManager;
        private readonly IUserApi _userApi;
        private readonly IRepository<UserRole> _userRoleRepo;
        private readonly IPasswordHasher _hasher;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;

        public DbInitializer ( ApplicationDbContext db , IRepository<AuthRole> roleManager , IRepository<AuthUser> userManager , IUserApi userApi , IRepository<UserRole> userRoleRepo , IPasswordHasher hasher , IRepository<Role> roleRepo , IRepository<User> userRepo )
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _userApi = userApi;
            _userRoleRepo = userRoleRepo;
            _hasher = hasher;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
        }

        public async void Execute ()
        {
            try
            {
                if ( _db.Database.GetPendingMigrations().Count() > 0 )
                {
                    _db.Database.Migrate();
                }
            }
            catch ( Exception ex )
            {

            }


            if ( _roleManager.GetCount(u => u.Title == "Managment") == 0 )
            {
                _roleManager.Add(new AuthRole(SD.Managment , "دسترسی کامل سیستم"));
                _roleManager.Add(new AuthRole(SD.Admin , "دسترسی به تمام اطلاعات به جز تعیین عضو کمیته"));
                _roleManager.Add(new AuthRole(SD.User , "بدون دسترسی"));
            }
            else
            {
                return;
            }

            var user = _userApi.GetUserAsync(SD.DefaultNationalCode).GetAwaiter().GetResult();

            _userManager.Add(new AuthUser(user.Mobile , user.Idmelli , user.Name , user.Lastname , user.Idmelli , _hasher.HashPassword(user.Idmelli)));
            _roleRepo.Add(new Role("مدیریت" , "مدیریت سیستم"));
            _userRoleRepo.Save();

            var authUser = _userManager.FirstOrDefault(u => u.UserName == user.Idmelli);
            var managerRole = _roleManager.FirstOrDefault(u => u.Title == SD.Managment);

            var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == "مدیریت");
            _userRepo.Add(new User(user.Name + " " + user.Lastname , user.Idmelli , DateTime.Now , DateTime.Now.AddYears(1) , role.Id));


            _userRoleRepo.Add(new UserRole(authUser.Id , managerRole.Id));
            _userRoleRepo.Save();
        }
    }
}
