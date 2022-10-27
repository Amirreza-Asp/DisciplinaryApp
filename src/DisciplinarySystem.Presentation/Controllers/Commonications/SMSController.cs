using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Commonications;
using DisciplinarySystem.Presentation.Controllers.Commonications.Models;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace DisciplinarySystem.Presentation.Controllers.Commonications
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class SMSController : Controller
    {
        private readonly ISmsService _smsService;
        private readonly IRepository<SMS> _SMSRepo;
        private readonly IRepository<AuthUser> _userRepo;

        public SMSController ( ISmsService smsService , IRepository<SMS> sMSRepo , IRepository<AuthUser> userRepo )
        {
            _smsService = smsService;
            _SMSRepo = sMSRepo;
            _userRepo = userRepo;
        }

        private static SMSFilter _filters = new SMSFilter();


        public async Task<IActionResult> Index ( SMSFilter filters )
        {
            _filters = filters;
            filters.Total = _SMSRepo.GetCount();
            var vm = new GetSMSList
            {
                Entities = await _SMSRepo.GetAllAsync(
                        include: source => source.Include(u => u.User) ,
                        orderBy: source => source.OrderByDescending(u => u.SendDate) ,
                        take: filters.Take , skip: filters.Skip) ,

                Filters = filters
            };
            return View(vm);
        }

        public IActionResult Send () => View();
        [HttpPost]
        public async Task<IActionResult> Send ( SMSDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            await _smsService.Send(command.Content , command.PhoneNumber);

            var userId = await _userRepo.FirstOrDefaultSelectAsync(
                filter: u => u.NationalCode.Value == User.FindFirstValue(ClaimTypes.NameIdentifier) ,
                select: u => u.Id);

            _SMSRepo.Add(new SMS(command.PhoneNumber , command.Content , userId));
            await _SMSRepo.SaveAsync();
            TempData[SD.Success] = "پیامک ارسال شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var sms = await _SMSRepo.FindAsync(id);
            if ( sms == null )
            {
                TempData[SD.Error] = "پیامک انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(sms);
        }


        public async Task<JsonResult> Remove ( Guid id )
        {
            var sms = await _SMSRepo.FindAsync(id);
            if ( sms == null )
                return Json(new { Success = false });

            sms.Delete();
            _SMSRepo.Update(sms);
            await _SMSRepo.SaveAsync();

            return Json(new { Success = true });
        }
    }
}
