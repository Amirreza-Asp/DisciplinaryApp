using AutoMapper;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Application.Violations.ViewModels.ViolationCategory;
using DisciplinarySystem.Presentation.Controllers.Violations.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Violations
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class ViolationCategoryController : Controller
    {
        private readonly IViolationCategoryService _service;
        private readonly IMapper _mapper;

        public ViolationCategoryController ( IViolationCategoryService service , IMapper mapper )
        {
            _service = service;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index ( ViolationCategoryFilter filters )
        {
            var vm = new GetAllViolationCategories
            {
                ViolationCategories = await _service.GetListAsync(skip: filters.Skip , take: filters.Take) ,
                TotalCount = _service.GetCount() ,
                Filters = filters
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "طبقه بندی وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index));
            }

            return View(entity);
        }

        public IActionResult Create () => View();
        [HttpPost]
        public async Task<IActionResult> Create ( CreateViolationCategory createViolationCategory )
        {
            if ( !ModelState.IsValid )
                return View(createViolationCategory);

            if ( await _service.GetByTitleAsync(createViolationCategory.Title) != null )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(createViolationCategory);
            }

            await _service.CreateAsync(createViolationCategory);
            return RedirectToAction(nameof(Index));
        }

        public async Task<ViewResult> Update ( Guid id )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "تخلف مورد نظر وجود ندارد";
                return View(nameof(Index));
            }

            var updateRole = _mapper.Map<UpdateViolationCategory>(entity);
            return View(updateRole);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateViolationCategory updateViolationCategory )
        {
            if ( !ModelState.IsValid )
                return View(updateViolationCategory);

            var entity = await _service.GetByTitleAsync(updateViolationCategory.Title);

            if ( entity != null && entity.Id != updateViolationCategory.Id )
            {
                TempData[SD.Warning] = "عنوان وارد شده تکراری است";
                return View(updateViolationCategory);
            }

            await _service.UpdateAsync(updateViolationCategory);
            TempData[SD.Success] = "ویرایش طبقه بندی تخلف با موفقیت انجام شد";
            return RedirectToAction(nameof(Index));
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            if ( await _service.RemoveAsync(id) )
                return Json(new { Success = true , Message = "عملیات با موفقیت انجام شد" });
            return Json(new { Success = false , Message = "عملیات با شکست مواجه شد" });
        }
    }
}
