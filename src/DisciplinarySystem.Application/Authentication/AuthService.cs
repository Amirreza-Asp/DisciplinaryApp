using DisciplinarySystem.Application.Authentication.Dtos;
using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text.Json;
using static DisciplinarySystem.Application.Authentication.Dtos.UserKhedmat;

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
            var user = await _authUserRepo.FirstOrDefaultAsync(
                u => u.UserName == command.UserName ,
                include: source =>
                    source.Include(u => u.Roles)
                               .ThenInclude(u => u.Role));

            if ( user != null )
            {
                if ( !_passwordHasher.VerifyPassword(user.Password , command.Password) )
                    return LoginResultDto.Faild("رمز وارد شده اشتباه است");


                var appUser = await _userRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == user.NationalCode.Value);
                if ( appUser == null || appUser.AttendenceTime.To.Date < DateTime.Now.Date )
                    return LoginResultDto.Faild("تاریخ حضور شما به پایان رسیده است");
                if ( appUser == null || appUser.AttendenceTime.From.Date > DateTime.Now.Date )
                    return LoginResultDto.Faild("تاریخ شروع کار شما شروع نشده");

                await AddClaimsAsync(user);
                return LoginResultDto.Successful();
            }


            var request = new HttpRequestMessage(HttpMethod.Post ,
                  $"https://khedmat.razi.ac.ir/api/KhedmatAPI/khedmat/users?action=auth&username={userName}&password={password}&requestUser={command.UserName}&requestPassword={command.Password}");

            var client = _clientFactory.CreateClient();

            var response = await client.SendAsync(request);

            if ( response.IsSuccessStatusCode )
            {
                var result = await response.Content.ReadAsStringAsync();
                try
                {
                    var data1 = JsonSerializer.Deserialize<KhedmatResponseStudent>(result);

                    AuthUser newUser = new AuthUser(data1.userInfo.phone , data1.userInfo.national_code , data1.userInfo.name
                        , data1.userInfo.last_name , data1.userInfo.username , _passwordHasher.HashPassword(command.Password));

                    _authUserRepo.Add(newUser);
                    await _authUserRepo.SaveAsync();
                    await AddClaimsAsync(newUser);
                }
                catch ( Exception ex )
                {
                    return LoginResultDto.Faild(ex.Message);
                }

                return LoginResultDto.Successful();
            }

            return LoginResultDto.Faild("نام کاربری یا رمز وارد شده اشتباه است");

        }

        private async Task AddClaimsAsync ( AuthUser user )
        {
            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.Name , user.Name + " " + user.Family));
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier , user.NationalCode.Value));

            if ( user.Roles != null )
            {
                foreach ( var item in user.Roles )
                {
                    identity.AddClaim(new Claim(ClaimTypes.Role , item.Role.Title));
                }
            }
            var principal = new ClaimsPrincipal(identity);
            await _contextAccessor.HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , principal);
        }

    }
}

