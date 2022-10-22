using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Presentation.Controllers.Commonications.Models;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Commonications
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class SMSController : Controller
    {
        private readonly ISmsService _smsService;

        public SMSController ( ISmsService smsService )
        {
            _smsService = smsService;
        }

        public IActionResult Send () => View();
        [HttpPost]
        public async Task<IActionResult> Send ( SMSDto command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            await _smsService.Send(command.Content , command.PhoneNumber);
            TempData[SD.Success] = "پیامک ارسال شد";
            return RedirectToAction(nameof(Send));
        }
    }
}
