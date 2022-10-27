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
        private readonly IPasswordHasher _hasher;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<Role> _roleRepo;

        public DbInitializer ( ApplicationDbContext db , IRepository<AuthRole> roleManager , IRepository<AuthUser> userManager , IUserApi userApi , IPasswordHasher hasher , IRepository<Role> roleRepo , IRepository<User> userRepo )
        {
            _db = db;
            _roleManager = roleManager;
            _userManager = userManager;
            _userApi = userApi;
            _hasher = hasher;
            _roleRepo = roleRepo;
            _userRepo = userRepo;
        }

        public async Task Execute ()
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


            if ( _roleManager.GetCount(u => u.Title == SD.Managment) == 0 )
            {
                _roleManager.Add(new AuthRole(SD.Managment , "دسترسی کامل سیستم"));
                _roleManager.Add(new AuthRole(SD.Admin , "دسترسی به تمام اطلاعات به جز تعیین عضو کمیته"));
                _roleManager.Add(new AuthRole(SD.User , "بدون دسترسی"));
            }
            else
            {
                return;
            }

            if ( _roleRepo.GetCount(u => u.Title == "مدیریت") == 0 )
            {
                _roleRepo.Add(new Role("مدیریت" , "دسترسی کامل"));
            }

            await _roleManager.SaveAsync();
            var adminRole = await _roleManager.FirstOrDefaultAsync(u => u.Title == SD.Managment);

            var user = _userApi.GetUserAsync(SD.DefaultNationalCode).GetAwaiter().GetResult();

            _userManager.Add(new AuthUser(user.Mobile , user.Idmelli , user.Name , user.Lastname , user.Idmelli , _hasher.HashPassword(user.Idmelli) , adminRole.Id));

            var managerRole = _roleManager.FirstOrDefault(u => u.Title == SD.Managment);

            var role = await _roleRepo.FirstOrDefaultAsync(u => u.Title == "مدیریت");
            _userRepo.Add(new User(user.Name + " " + user.Lastname , user.Idmelli , DateTime.Now , DateTime.Now.AddYears(2) , role.Id , SD.Tajdid));

            await _userManager.SaveAsync();
        }
    }
}
