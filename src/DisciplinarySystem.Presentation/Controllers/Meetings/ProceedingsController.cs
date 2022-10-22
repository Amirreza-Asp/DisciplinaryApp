using DisciplinarySystem.Application.Meetings.Interfaces;
using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Meetings
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class ProceedingsController : Controller
    {

        private readonly IProceedingsService _prcService;

        public ProceedingsController ( IProceedingsService prcService )
        {
            _prcService = prcService;
        }

        public async Task<IActionResult> Details ( Guid meetingId )
        {
            var entity = await _prcService.GetByMeetingIdAsync(meetingId);

            if ( entity == null )
            {
                TempData[SD.Warning] = "صورت جلسه انتخاب شده وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            return View(entity);
        }

        public IActionResult Create ( Guid meetingId )
        {
            var command = new CreateProceedings
            {
                MeetingId = meetingId ,
                RequestPath = Request.GetTypedHeaders().Referer.ToString()
            };
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateProceedings command )
        {
            if ( !ModelState.IsValid )
            {
                return View(command);
            }

            await _prcService.CreateAsync(command);
            TempData[SD.Success] = "صورت جلسه با موفقیت ذخیره شد";
            return Redirect(command.RequestPath);
        }

        public async Task<IActionResult> Update ( Guid meetingId )
        {
            var entity = await _prcService.GetByMeetingIdAsync(meetingId);

            if ( entity == null )
            {
                TempData[SD.Warning] = "صورت جلسه انتخاب شده وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }


            var command = UpdateProccedings.Create(entity);
            command.RequestPath = Request.GetTypedHeaders().Referer.ToString();
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateProccedings command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            await _prcService.UpdateAsync(command);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return Redirect(command.RequestPath);
        }

        public async Task<JsonResult> Remove ( Guid id ) => Json(new { Success = await _prcService.RemoveByMeetingIdAsync(id) });
    }
}
