using DisciplinarySystem.Application.Authentication.Dtos;
using DisciplinarySystem.Application.Authentication.Interfaces;
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
using Microsoft.EntityFrameworkCore;
using System.Data;
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
        private readonly IRepository<Role> _roleRepo;
        private readonly IPasswordHasher _passHasher;

        private static UserFilter _filters = new UserFilter();

        public UserController ( IUserService userService , IUserApi userApi , IRepository<AuthRole> authRoleRepo , IPositionAPI positionApi , IRepository<AuthUser> authUserRepo , IRepository<User> userRepo , IRepository<Role> roleRepo , IPasswordHasher passHasher )
        {
            _userService = userService;
            _userApi = userApi;
            _authRoleRepo = authRoleRepo;
            _positionApi = positionApi;
            _authUserRepo = authUserRepo;
            _userRepo = userRepo;
            _roleRepo = roleRepo;
            _passHasher = passHasher;
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

            await _userService.CreateAsync(command);
            TempData[SD.Success] = "عضو جدید به کمیته اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var user = await _userRepo.FirstOrDefaultAsync(
                filter: entity => entity.Id == id ,
                include: source => source
                .Include(u => u.Role)
                .Include(u => u.AuthUser)
                    .ThenInclude(u => u.Role));

            if ( user == null )
            {
                TempData[SD.Error] = "کاربر مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }
            if ( user.AuthUser == null )
            {
                var authUser = await _authUserRepo.FirstOrDefaultAsync(u =>
                        u.NationalCode.Value == user.NationalCode.Value && u.UserId == null);

                if ( authUser != null )
                {
                    authUser.WithUserId(user.Id);
                    _authUserRepo.Update(authUser);
                    await _authUserRepo.SaveAsync();
                    user = await _userRepo.FirstOrDefaultAsync(
                            filter: entity => entity.Id == id ,
                                include: source => source
                                .Include(u => u.Role)
                                .Include(u => u.AuthUser)
                                .ThenInclude(u => u.Role));
                }
            }


            var command = UpdateUser.Create(user);
            command.Access = user.AuthUser == null ? default : user.AuthUser.Role.Id;
            command = await FillUpdateUserInfo(command);
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateUser command )
        {
            PersianCalendar pc = new PersianCalendar();
            command.StartDate = command.StartDate.ToMiladi();
            command.EndDate = command.EndDate.ToMiladi();
            var roleId = await _authUserRepo.FirstOrDefaultSelectAsync<long>(
                filter: u => u.User.Id == command.Id ,
                select: s => s.RoleId);


            if ( !ModelState.IsValid )
            {
                command.Access = roleId;
                command = await FillUpdateUserInfo(command);
                return View(command);
            }

            var info = await _userApi.GetUserAsync(command.NationalCode.ToString());
            if ( info == null )
            {
                command.Access = roleId;
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


        public async Task<IActionResult> HandAdd ()
        {
            var dto = new HandAdd
            {
                AuthRoles = await _authRoleRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title , Value = u.Id.ToString() }) ,
                Roles = await _roleRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title , Value = u.Id.ToString() })
            };

            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> HandAdd ( HandAdd command )
        {
            if ( !ModelState.IsValid )
            {
                command.AuthRoles = await _authRoleRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title , Value = u.Id.ToString() });
                command.Roles = await _roleRepo.GetAllAsync(select: u => new SelectListItem { Text = u.Title , Value = u.Id.ToString() });
                return View(command);
            }

            var user = new User(command.Name + " " + command.Family , command.NationalCode , DateTime.Now.AddYears(1) , DateTime.Now.AddYears(1) , command.RoleId , command.Type);
            var authUser = new AuthUser(command.PhoneNumber , command.NationalCode , command.Name ,
                command.Family , command.NationalCode , _passHasher.HashPassword(command.NationalCode) , command.Access , user.Id);

            _userRepo.Add(user);
            _authUserRepo.Add(authUser);
            await _userRepo.SaveAsync();

            TempData[SD.Success] = "عضو با موفقیت افزوده شد";
            return RedirectToAction(nameof(HandAdd));
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
