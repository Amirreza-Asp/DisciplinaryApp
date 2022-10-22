using DisciplinarySystem.Application.Authentication.Dtos;
using DisciplinarySystem.Application.Authentication.Interfaces;
using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace DisciplinarySystem.Presentation.Controllers.Authentication
{
    public class AccountController : Controller
    {
        private readonly IAuthService _authService;
        private readonly ISmsService _smsService;
        private readonly IRepository<AuthUser> _userRepo;

        public AccountController ( IAuthService authService , IRepository<AuthUser> userRepo , ISmsService smsService )
        {
            _authService = authService;
            _userRepo = userRepo;
            _smsService = smsService;
        }

        public IActionResult Login () => View();
        [HttpPost]
        public async Task<IActionResult> Login ( LoginDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var res = await _authService.LoginAsync(command);
            if ( res.Success )
            {
                TempData[SD.Success] = $"{User.Identity.Name} خوش امدید";
                return RedirectToAction("Index" , "Home");
            }

            TempData[SD.Error] = res.Message.ToString();
            return View(command);
        }


        public async Task<IActionResult> Logout ()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        [Authorize]
        public IActionResult ChangePassword ()
        {
            return View();
        }
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> ChangePassword ( ChangePasswordDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var nationalCode = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if ( nationalCode == null )
            {
                TempData[SD.Error] = "عملیات با شکست مواجه شد";
                return View(command);
            }

            await _authService.ChangePasswordAsync(command);
            TempData[SD.Success] = "رمز عبور با موفقیت تغییر یافت";
            return RedirectToAction(nameof(Logout));
        }


        public IActionResult ForgetPassword () => View();
        [HttpPost]
        public async Task<IActionResult> ForgetPassword ( ForgetPasswordDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var user = await _userRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == command.NationalCode);

            if ( user == null )
            {
                TempData[SD.Error] = "کد ملی وارد شده در سیستم وجود ندارد";
                return View(command);
            }

            Random rnd = new Random();
            command.SecretCode = rnd.Next(10000 , 99999);
            HttpContext.Session.Set<ForgetPasswordDto>(SD.ForgetPassword , command);


            await _smsService.Send($"کد فراموشی سامانه انضباطی : {command.SecretCode}" , user.PhoneNumber.Value);
            TempData[SD.Info] = "کد از طریق پیامک ارسال شد";
            return RedirectToAction(nameof(ReciveCode));
        }

        public IActionResult ReciveCode () => View();
        [HttpPost]
        public async Task<IActionResult> ReciveCode ( ReciveCodeDto command )
        {
            var dto = HttpContext.Session.Get<ForgetPasswordDto>(SD.ForgetPassword);
            if ( command.Code != dto.SecretCode.ToString() )
            {
                TempData[SD.Error] = "کد وارد شده اشتباه است";
                return View(command);
            }

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier , dto.NationalCode));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme , principal);
            HttpContext.Session.Remove(SD.ForgetPassword);

            return RedirectToAction(nameof(ChangePassword));
        }

    }
}
