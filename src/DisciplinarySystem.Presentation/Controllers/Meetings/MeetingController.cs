using Ardalis.Specification;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Meetings.Interfaces;
using DisciplinarySystem.Application.Meetings.ViewModels;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Presentation.Controllers.Meetings.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Meetings
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class MeetingController : Controller
    {
        private readonly IMeetingService _meetService;
        private readonly IUserService _userService;

        private static MeetingFilter _filters = new MeetingFilter();
        private static DateTime _dateTime = DateTime.Now;

        public MeetingController ( IMeetingService meetService , IUserService userService )
        {
            _meetService = meetService;
            _userService = userService;
        }

        public async Task<IActionResult> Index ( MeetingFilter filters )
        {
            _filters = filters;

            filters.Move();
            filters.RoundMeetingsDate();

            var LastDateOfTheMonth = filters.MeetingsDate.GetTheLastDateOfTheMonth();

            var vm = new GetAllMeetings
            {
                MeetingFilter = filters ,
                Meetings = await _meetService.ListAsync(
                    filters: u =>
                    u.HoldingTime.From.Date >= filters.MeetingsDate.Date &&
                    u.HoldingTime.From.Date <= LastDateOfTheMonth.Date)
            };
            return View(vm);
        }


        public async Task<IActionResult> GetAll ( MeetingFilter filters )
        {
            _filters = filters;
            var vm = new GetAllMeetings
            {
                MeetingFilter = filters ,
                Meetings = await _meetService.ListAsync(
                        u => u.HoldingTime.From.Date.Equals(filters.MeetingsDate.Date))
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var meeting = await _meetService.GetByIdAsync(id);
            if ( meeting == null )
            {
                TempData[SD.Error] = "جلسه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(GetAll) , _filters);
            }

            return View(meeting);
        }

        public async Task<IActionResult> Create ( DateTime meetingDate )
        {
            var command = new CreateMeeting
            {
                Users = await _userService.GetSelectedUsersAsync() ,
                MeetingDate = meetingDate
            };
            return View(command);

        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateMeeting command )
        {
            if ( !ModelState.IsValid )
            {
                command.Users = await _userService.GetSelectedUsersAsync();
                return View(command);
            }

            if ( command.GetStartTime() > command.GetEndTime() )
            {
                command.Users = await _userService.GetSelectedUsersAsync();
                TempData[SD.Warning] = "زمان پایان نمیتواند از شروع کمتر باشد";
                return View(command);
            }

            await _meetService.CreateAsync(command);
            TempData[SD.Success] = "جلسه با موفقیت اضافه شد";
            return RedirectToAction(nameof(GetAll) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var entity = await _meetService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "جلسه انتخاب شده وجود ندارد";
                return RedirectToAction(nameof(GetAll) , _filters);
            }

            var command = UpdateMeeting.Create(entity);
            command.SelectedUsers = new List<(string Name, Guid Id)>();

            entity.MeetingUsers.ToList().ForEach(item => command.SelectedUsers.Add(new(item.User.FullName , item.UserId)));

            command.Users = await _userService.GetSelectedUsersAsync();
            command.Users = command.Users.Where(u => !entity.MeetingUsers.Select(u => u.UserId).Contains(Guid.Parse(u.Value)));
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateMeeting command )
        {
            if ( !ModelState.IsValid || command.GetStartTime() > command.GetEndTime() )
            {
                if ( command.GetStartTime() > command.GetEndTime() )
                    TempData[SD.Warning] = "زمان پایان نمیتواند از شروع کمتر باشد";

                var entity = await _meetService.GetByIdAsync(command.Id);
                command.SelectedUsers = new List<(string Name, Guid Id)>();

                entity.MeetingUsers.ToList().ForEach(item => command.SelectedUsers.Add(new(item.User.FullName , item.UserId)));
                command.Users = await _userService.GetSelectedUsersAsync();
                command.Users = command.Users.Where(u => !entity.MeetingUsers.Select(u => u.UserId).Contains(Guid.Parse(u.Value)));
                return View(command);
            }

            await _meetService.UpdateAsync(command);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(GetAll) , _filters);
        }


        public async Task<JsonResult> RemoveUser ( Guid id )
        {
            var res = await _meetService.RemoveUserAsync(id);
            return Json(new { Success = res });
        }

        public async Task<JsonResult> Remove ( Guid id ) => Json(new { Success = await _meetService.RemoveAsync(id) });
    }
}
