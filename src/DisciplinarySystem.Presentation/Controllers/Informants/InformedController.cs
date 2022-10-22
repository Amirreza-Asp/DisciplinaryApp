using DisciplinarySystem.Application.Informants.Interfaces;
using DisciplinarySystem.Application.Informants.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Informants.ViewModels;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers.Informants
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class InformedController : Controller
    {
        private readonly IInformedService _service;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IUserApi _userApi;

        private static InformedFilter _filters = new InformedFilter();

        public InformedController ( IInformedService service , IWebHostEnvironment hostEnv , IUserApi userApi )
        {
            _service = service;
            _hostEnv = hostEnv;
            _userApi = userApi;
        }

        public async Task<IActionResult> Index ( InformedFilter filters )
        {

            _filters = filters;
            PersianCalendar pc = new PersianCalendar();
            if ( filters.CreateDate != default )
                filters.CreateDate = new DateTime(filters.CreateDate.Year , filters.CreateDate.Month , filters.CreateDate.Day , pc);


            var vm = new GetInformants
            {
                Informants = await GetFilteredInformants(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _service.GetByIdAsync(id);
            return View(entity);
        }

        public IActionResult Create ( long caseId )
        {
            var createVM = new CreateInformed { CaseId = caseId };
            return View(createVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateInformed command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            await _service.CreateAsync(command , files);
            TempData[SD.Success] = "مطلع با موفقیت اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Update ( Guid id )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "اظهار مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(UpdateInformed.Create(entity));
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateInformed command )
        {
            if ( !ModelState.IsValid )
            {
                command.CurrentDocuments = await _service.GetDocuments(command.Id);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _service.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> RemoveFile ( Guid id )
        {
            return Json(new { Success = await _service.RemoveFileAsync(id) });
        }

        public async Task<JsonResult> Remove ( Guid id )
        {
            return Json(new { Success = await _service.RemoveAsync(id) });
        }

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.InformedDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }

        public async Task<JsonResult> GetInformedInfo ( String id )
        {
            var user = await _userApi.GetUserAsync(id);
            if ( user == null )
                return Json(new { Exists = false });

            return Json(new
            {
                Exists = true ,
                Data = new
                {
                    FullName = user.Name + " " + user.Lastname ,
                    PhoneNumber = user.Mobile ,
                    Father = user.Fathername
                }
            });
        }

        #region Utilities
        private async Task<IEnumerable<InformedDetails>> GetFilteredInformants ( InformedFilter filters )
        {
            return await _service.GetListAsync(
                                    filter: entity => ( String.IsNullOrEmpty(filters.Subject) ||
                                                        entity.Subject.Contains(filters.Subject) ) &&
                                                     ( String.IsNullOrEmpty(filters.FullName) ||
                                                        entity.FullName.Contains(filters.FullName) ) &&
                                                     ( filters.CreateDate == default ||
                                                        entity.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                                        entity.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                                        entity.CreateDate.Day.Equals(filters.CreateDate.Day) ) &&
                                                      entity.CaseId == filters.CaseId ,
                                                       skip: filters.Skip ,
                                                       take: filters.Take);
        }

        private int GetFilteredCount ( InformedFilter filters )
        {
            return _service.GetCount(
                                    filter: entity => ( String.IsNullOrEmpty(filters.Subject) ||
                                                        entity.Subject.Contains(filters.Subject) ) &&
                                                     ( String.IsNullOrEmpty(filters.FullName) ||
                                                        entity.FullName.Contains(filters.FullName) ) &&
                                                     ( filters.CreateDate == default ||
                                                        entity.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                                        entity.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                                        entity.CreateDate.Day.Equals(filters.CreateDate.Day) ) &&
                                                      entity.CaseId == filters.CaseId);
        }
        #endregion
    }
}
