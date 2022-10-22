using AutoMapper;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Users.ViewModels.Role;
using DisciplinarySystem.Presentation.Controllers.Users.ViewModels.Roles;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Users
{
    [Authorize(Roles = $"{SD.Managment}")]
    public class RoleController : Controller
    {
        private readonly IRoleService _service;
        private readonly IMapper _mapper;

        private static RoleFilter _filters = new RoleFilter();

        public RoleController ( IRoleService service , IMapper mapper )
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index ( RoleFilter filters )
        {
            _filters = filters;
            var vm = new GetAllRoles
            {
                Roles = await _service.GetListAsync(skip: filters.Skip , take: filters.Take) ,
                TotalCount = _service.GetCount() ,
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var role = await _service.GetByIdAsync(id);
            if ( role == null )
            {
                TempData[SD.Success] = "نقش مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(role);
        }

        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create ( CreateRole createRole )
        {
            if ( !ModelState.IsValid )
                return View(createRole);

            if ( await _service.GetByTitleAsync(createRole.Title) != null )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(createRole);
            }

            await _service.CreateAsync(createRole);
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<ViewResult> Update ( Guid id )
        {
            var role = await _service.GetByIdAsync(id);
            if ( role == null )
            {
                TempData[SD.Error] = "ایتم مورد نظر وجود ندارد";
                return View(nameof(Index) , _filters);
            }

            var updateRole = _mapper.Map<UpdateRole>(role);
            return View(updateRole);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateRole updateRole )
        {
            if ( !ModelState.IsValid )
                return View(updateRole);

            if ( await _service.GetByTitleAsync(updateRole.Title) != null )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(updateRole);
            }

            await _service.UpdateAsync(updateRole);
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            if ( await _service.RemoveAsync(id) )
                return Json(new { Success = true , Message = "عملیات با موفقیت انجام شد" });
            return Json(new { Success = false , Message = "عملیات با شکست مواجه شد" });
        }
    }
}
