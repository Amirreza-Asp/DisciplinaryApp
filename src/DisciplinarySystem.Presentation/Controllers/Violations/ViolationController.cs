using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Application.Violations.ViewModels.Violation;
using DisciplinarySystem.Presentation.Controllers.Violations.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers.Violations
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class ViolationController : Controller
    {
        private readonly IViolationService _service;
        private readonly IViolationCategoryService _categoryService;
        private readonly IWebHostEnvironment _hostEnv;

        private static ViolationFilters _filters = new ViolationFilters();

        public ViolationController ( IViolationService service , IViolationCategoryService categoryService , IWebHostEnvironment hostEnv )
        {
            _service = service;
            _categoryService = categoryService;
            _hostEnv = hostEnv;
        }

        public async Task<IActionResult> Index ( ViolationFilters filters )
        {
            _filters = filters;
            filters.Categories = await GetCategories();

            PersianCalendar pc = new PersianCalendar();
            if ( filters.CreateDate != default )
                filters.CreateDate = new DateTime(filters.CreateDate.Year , filters.CreateDate.Month , filters.CreateDate.Day , pc);

            var vm = new GetAllViolations
            {
                Violations = await GetFilteredViolations(filters) ,
                Filters = filters ,
                TotalCount = GetFilteredCount(filters)
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _service.GetByIdAsync(id);
            return View(entity);
        }

        public async Task<IActionResult> Create ( long caseId )
        {
            var violation = new CreateViolation { Categories = await GetCategories() , CaseId = caseId };
            return View(violation);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateViolation createViolation )
        {
            if ( !ModelState.IsValid )
            {
                createViolation.Categories = await GetCategories();
                return View(createViolation);
            }
            var files = HttpContext.Request.Form.Files;

            await _service.CreateAsync(createViolation , files);

            TempData[SD.Success] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            if ( await _service.RemoveAsync(id) )
                return Json(new { Success = true , Message = "ایتم مورد نظر با موفقیت حذف شد" });
            return Json(new { Success = false });
        }

        public async Task<IActionResult> Update ( Guid id )
        {
            var violation = await _service.GetByIdAsync(id);
            if ( violation == null )
            {
                TempData[SD.Error] = "تخلف مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var updateViolation = new UpdateViolation
            {
                Id = violation.Id ,
                CaseId = violation.CaseId ,
                CreateDate = violation.CreateDate ,
                CategoryId = violation.CategoryId ,
                Definition = violation.Definition ,
                Title = violation.Title ,
                CurrentDocuments = violation.Documents.ToList() ,
                Categories = await GetCategories()
            };
            return View(updateViolation);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateViolation updateViolation )
        {
            if ( !ModelState.IsValid )
            {
                var violation = await _service.GetByIdAsync(updateViolation.Id);
                updateViolation.CurrentDocuments = violation.Documents.ToList();
                updateViolation.Categories = await GetCategories();

                return View(updateViolation);
            }

            var files = HttpContext.Request.Form.Files;
            await _service.UpdateAsync(updateViolation , files);

            TempData[SD.Success] = "عملیات با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> RemoveFile ( Guid id )
        {
            if ( await _service.RemoveDocument(id) )
                return Json(new { Success = true , Message = "ایتم با موفقیت حذف شد" });
            return Json(new { Success = true , Message = "حذف با شکست مواجه شد" });
        }

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.ViolationDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        #region Utilities
        private async Task<List<GetViolatonDetails>> GetFilteredViolations ( ViolationFilters filters )
        {
            return await _service.GetAllAsync(
                filter: entity => ( String.IsNullOrEmpty(filters.Title) || entity.Title.Equals(filters.Title) ) &&
                                    ( filters.Category == default || entity.Category.Id.Equals(filters.Category) ) &&
                                    ( filters.CreateDate == default ||
                                        ( entity.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                        entity.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                        entity.CreateDate.Day.Equals(filters.CreateDate.Day) ) ) &&
                                    ( filters.CaseId.Equals(entity.CaseId) ) ,
                                    skip: filters.Skip ,
                                    take: filters.Take);
        }
        private int GetFilteredCount ( ViolationFilters filters )
        {
            return _service.GetCount(
                filter: entity => ( String.IsNullOrEmpty(filters.Title) || entity.Title.Equals(filters.Title) ) &&
                                    ( filters.Category == default || entity.Category.Id.Equals(filters.Category) ) &&
                                    ( filters.CreateDate == default ||
                                        ( entity.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                        entity.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                        entity.CreateDate.Day.Equals(filters.CreateDate.Day) ) ) &&
                                    ( filters.CaseId.Equals(entity.CaseId) ));
        }

        private async Task<List<SelectListItem>> GetCategories ()
        {
            var categories = await _categoryService.GetListAsync();
            if ( categories == null )
                return new List<SelectListItem>();
            return categories.Select(x => new SelectListItem { Text = x.Title , Value = x.Id.ToString() }).ToList();
        }
        #endregion
    }
}
