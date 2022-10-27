using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.User;
using DisciplinarySystem.Domain.Authentication;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Presentation.Controllers.Users.ViewModels;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers.Users
{
    [Authorize(Roles = $"{SD.Managment}")]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly IUserApi _userApi;
        private readonly IPositionAPI _positionApi;
        private readonly IRepository<AuthRole> _authRoleRepo;
        private readonly IRepository<AuthUser> _authUserRepo;
        private readonly IRepository<User> _userRepo;

        private static UserFilter _filters = new UserFilter();

        public UserController ( IUserService userService , IUserApi userApi , IRepository<AuthRole> authRoleRepo , IPositionAPI positionApi , IRepository<AuthUser> authUserRepo , IRepository<User> userRepo )
        {
            _userService = userService;
            _userApi = userApi;
            _authRoleRepo = authRoleRepo;
            _positionApi = positionApi;
            _authUserRepo = authUserRepo;
            _userRepo = userRepo;
        }

        public async Task<IActionResult> Index ( UserFilter filters )
        {
            _filters = filters;
            filters.StartDate = filters.StartDate.ToMiladi();
            filters.EndDate = filters.EndDate.ToMiladi();
            filters.Roles = await _userService.GetSelectListRolesAsync();

            var vm = new GetAllUsers
            {
                Users = await GetFilteredUsers(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var user = await _userService.GetById(id);
            return View(user);
        }

        public async Task<IActionResult> Create ()
        {
            var command = new CreateUser();
            command = await FillUserInfo(command);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateUser command )
        {
            PersianCalendar pc = new PersianCalendar();
            command.StartDate = command.StartDate == default ? default : new DateTime(command.StartDate.Year , command.StartDate.Month , command.StartDate.Day , pc);
            command.EndDate = command.EndDate == default ? default : new DateTime(command.EndDate.Year , command.EndDate.Month , command.EndDate.Day , pc);

            if ( !ModelState.IsValid )
            {
                command = await FillUserInfo(command);
                return View(command);
            }

            var info = await _userApi.GetUserAsync(command.NationalCode.ToString());
            if ( info == null )
            {
                TempData[SD.Error] = "کد ملی وارد شده در سیستم وجود ندارد";
                command = await FillUserInfo(command);
                return View(command);
            }

            if ( String.IsNullOrEmpty(info.Mobile) )
            {
                TempData[SD.Warning] = $"شماره تلفن {String.Concat(info.Name + " " + info.Lastname)} در سیستم ثبت نشده است";
                command = await FillUserInfo(command);
                return View(command);
            }

            await _userService.CreateAsync(command);
            TempData[SD.Success] = "عضو جدید به کمیته اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var user = await _userService.GetById(id);
            if ( user == null )
            {
                TempData[SD.Error] = "کاربر مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }


            var command = UpdateUser.Create(user);
            command.Access = await _authUserRepo.FirstOrDefaultSelectAsync<long>(filter: u => u.NationalCode.Value == user.NationalCode.Value , select: s => s.RoleId);
            command = await FillUpdateUserInfo(command);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateUser command )
        {
            PersianCalendar pc = new PersianCalendar();
            command.StartDate = command.StartDate.ToMiladi();
            command.EndDate = command.EndDate.ToMiladi();
            var roleId = await _authUserRepo.FirstOrDefaultSelectAsync<long>(filter: u => u.NationalCode.Value == command.NationalCode , select: s => s.RoleId);
            command.Access = roleId;


            if ( !ModelState.IsValid )
            {
                command = await FillUpdateUserInfo(command);
                return View(command);
            }

            var info = await _userApi.GetUserAsync(command.NationalCode.ToString());
            if ( info == null )
            {
                command = await FillUpdateUserInfo(command);
                TempData[SD.Error] = "کد ملی وارد شده در سیستم وجود ندارد";
                return View(command);
            }

            await _userService.UpdateAsync(command);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            var user = await _userRepo.FindAsync(id);
            if ( user == null )
                return Json(new { Success = false });

            var authUser = await _authUserRepo.FirstOrDefaultAsync(u => u.NationalCode.Value == user.NationalCode.Value);
            if ( authUser != null )
                _authUserRepo.Remove(authUser);

            _userRepo.Remove(user);
            await _userRepo.SaveAsync();
            return Json(new { Success = true });
        }


        public async Task<JsonResult> GetUserInfo ( String id )
        {
            var user = await _userApi.GetUserAsync(id);
            if ( user == null )
                return Json(new { Exists = false });

            return Json(new
            {
                Exists = true ,
                Info = new
                {
                    FullName = $"{user.Name} {user.Lastname}" ,
                    NationalCode = user.Idmelli ,
                    Email = user.Email ,
                    PhoneNumber = user.Mobile
                }
            });
        }

        private async Task<IEnumerable<User>> GetFilteredUsers ( UserFilter filters )
        {
            return await _userService.GetListAsync(
                    filter: entity => ( String.IsNullOrEmpty(filters.FullName) ||
                                    entity.FullName.Contains(filters.FullName) ) &&
                                    ( !filters.RoleId.HasValue || entity.RoleId.Equals(filters.RoleId.Value) ) &&
                                    ( filters.StartDate == default ||
                                    entity.AttendenceTime.From.Year.Equals(filters.StartDate.Year) &&
                                    entity.AttendenceTime.From.Month.Equals(filters.StartDate.Month) &&
                                    entity.AttendenceTime.From.Day.Equals(filters.StartDate.Day) ) &&
                                    ( filters.EndDate == default ||
                                    entity.AttendenceTime.To.Year.Equals(filters.EndDate.Year) &&
                                    entity.AttendenceTime.To.Month.Equals(filters.EndDate.Month) &&
                                    entity.AttendenceTime.To.Day.Equals(filters.EndDate.Day) ) ,
                            skip: filters.Skip ,
                            take: filters.Take);
        }

        private int GetFilteredCount ( UserFilter filters )
        {
            return _userService.GetCount(
                    filter: entity => ( String.IsNullOrEmpty(filters.FullName) ||
                                    entity.FullName.Contains(filters.FullName) ) &&
                                    ( !filters.RoleId.HasValue || entity.RoleId.Equals(filters.RoleId.Value) ) &&
                                    ( filters.StartDate == default ||
                                    entity.AttendenceTime.From.Year.Equals(filters.StartDate.Year) &&
                                    entity.AttendenceTime.From.Month.Equals(filters.StartDate.Month) &&
                                    entity.AttendenceTime.From.Day.Equals(filters.StartDate.Day) ) &&
                                    ( filters.EndDate == default ||
                                    entity.AttendenceTime.To.Year.Equals(filters.EndDate.Year) &&
                                    entity.AttendenceTime.To.Month.Equals(filters.EndDate.Month) &&
                                    entity.AttendenceTime.To.Day.Equals(filters.EndDate.Day) ));
        }

        private async Task<CreateUser> FillUserInfo ( CreateUser command )
        {
            command.Roles = await _userService.GetSelectListRolesAsync();
            command.AuthRoles = await _authRoleRepo.GetAllAsync<SelectListItem>(select: entity => new SelectListItem { Text = $"{entity.PersianTitle()} ({entity.Description})" , Value = entity.Id.ToString() });

            var positions = await _positionApi.GetPositionsAsync();
            command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
            return command;
        }

        private async Task<UpdateUser> FillUpdateUserInfo ( UpdateUser command )
        {
            command.Roles = await _userService.GetSelectListRolesAsync();
            command.AuthRoles = await _authRoleRepo.GetAllAsync<SelectListItem>(select: entity => new SelectListItem { Text = $"{entity.PersianTitle()} ({entity.Description})" , Value = entity.Id.ToString() });
            var positions = await _positionApi.GetPositionsAsync();
            command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
            return command;
        }
    }
}
