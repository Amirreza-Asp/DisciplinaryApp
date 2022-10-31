using DisciplinarySystem.Application.Authentication.Dtos;
using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisciplinarySystem.Application.Authentication
{
    public class AuthService : IAuthService
    {
        private readonly IRepository<AuthUser> _authUserRepo;
        private readonly IRepository<User> _userRepo;
        private readonly IRepository<AuthRole> _roleRepo;
        private readonly IHttpClientFactory _clientFactory;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IHttpContextAccessor _contextAccessor;

        private readonly string userName = "sportprog";
        private readonly string password = "i2Zu33gO$I";

        public AuthService ( IRepository<AuthUser> authUserRepo , IRepository<User> userRepo , IRepository<AuthRole> roleRepo , IHttpClientFactory clientFactory , IPasswordHasher passwordHasher , IHttpContextAccessor contextAccessor )
        {
            _authUserRepo = authUserRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _clientFactory = clientFactory;
            _passwordHasher = passwordHasher;
            _contextAccessor = contextAccessor;
        }

        public async Task ChangePasswordAsync ( ChangePasswordDto command )
        {
            var nationalCode = _contextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier);
            if ( nationalCode == null )
                return;

            var user = await _authUserRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == nationalCode.Value);
            user.WithPassword(_passwordHasher.HashPassword(command.Password));
            _authUserRepo.Update(user);
            await _authUserRepo.SaveAsync();
        }

        public async Task<LoginResultDto> LoginAsync ( LoginDto command )
        {
            var users = await _authUserRepo.GetAllAsync(
                u => u.UserName == command.UserName ,
                include: source =>
                    source.Include(u => u.Role));

            if ( users != null )
            {
                var expectedUsers = users.ToList().Where(u => _passwordHasher.VerifyPassword(u.Password , command.Password));



                if ( !expectedUsers.Any() )
                    return LoginResultDto.Faild("رمز وارد شده اشتباه است");


                var appUsers = await _userRepo.GetAllAsync(u => u.NationalCode.Value == command.UserName);
                if ( appUsers == null || appUsers.All(u => u.AttendenceTime.To.Date < DateTime.Now.Date) )
                    return LoginResultDto.Faild("تاریخ حضور شما به پایان رسیده است");
                if ( appUsers == null || appUsers.All(u => u.AttendenceTime.From.Date > DateTime.Now.Date) )
                    return LoginResultDto.Faild("تاریخ شروع کار شما شروع نشده");

                var userWithHeightRole = users.ToList().Where(u => u.Role.Title == SD.Managment).FirstOrDefault();
                if ( userWithHeightRole == null )
                    userWithHeightRole = users.ToList().Where(u => u.Role.Title == SD.Admin).FirstOrDefault();
                if ( userWithHeightRole == null )
                    userWithHeightRole = users.ToList().First();

                await AddClaimsAsync(userWithHeightRole);
                return LoginResultDto.Successful();
            }

            return LoginResultDto.Faild("نام کاربری وارد شده اشتباه است");

        }

        private async Task AddClaimsAsync ( AuthUser user )
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name , user.Name + " " + user.Family));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier , user.NationalCode.Value));

            if ( user.Role != null )
            {
                identity.AddClaim(new Claim(ClaimTypes.Role , user.Role.Title));
            }
            var principal = new ClaimsPrincipal(identity);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , principal);
        }

    }
}

