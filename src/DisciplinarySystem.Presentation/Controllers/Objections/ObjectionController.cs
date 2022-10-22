using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Objections.Interfaces;
using DisciplinarySystem.Application.Objections.ViewModels;
using DisciplinarySystem.Domain.Objections;
using DisciplinarySystem.Presentation.Controllers.Objections.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Objections
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class ObjectionController : Controller
    {
        private readonly IObjectionService _objService;
        private readonly IComplainingService _cmpService;
        private readonly IWebHostEnvironment _hostEnv;

        private static ObjectionFilter _filters = new ObjectionFilter();

        public ObjectionController ( IObjectionService objService , IComplainingService cmpService , IWebHostEnvironment hostEnv )
        {
            _objService = objService;
            _cmpService = cmpService;
            _hostEnv = hostEnv;
        }

        public async Task<IActionResult> Index ( ObjectionFilter filters )
        {
            _filters = filters;
            filters.CreateDate = filters.CreateDate.ToMiladi();
            var vm = new GetAllObjections
            {
                Objections = await GetFilteredObjections(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var defence = await _objService.GetByIdAsync(id);
            if ( defence == null )
            {
                TempData[SD.Warning] = "اعتراض مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(defence);
        }

        public IActionResult Create ( long caseId )
        {
            var vm = new CreateObjection
            {
                CaseId = caseId ,
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateObjection command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            await _objService.CreateAsync(command , files);
            TempData[SD.Success] = "اعتراض با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var obj = await _objService.GetByIdAsync(id);
            if ( obj == null )
            {
                TempData[SD.Warning] = "اعتراض مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var vm = UpdateObjection.Create(obj);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateObjection command )
        {
            if ( !ModelState.IsValid )
            {
                command.CurrentDocuments = ( List<ObjectionDocument>? ) await _objService.GetDocumentsAsync(command.Id);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _objService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<JsonResult> Remove ( Guid id ) => Json(new { Success = await _objService.RemoveAsync(id) });

        public async Task<JsonResult> RemoveFile ( Guid id ) => Json(new { Success = await _objService.RemoveFileAsync(id) });


        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.ObjectionDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        private async Task<IEnumerable<ObjectionDetails>> GetFilteredObjections ( ObjectionFilter filters )
        {
            return await _objService.ListAsync(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                              ( String.IsNullOrEmpty(filters.Result) || entity.Result.Equals(filters.Result) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId ,
                            skip: filters.Skip ,
                            take: filters.Take);
        }
        private int GetFilteredCount ( ObjectionFilter filters )
        {
            return _objService.GetCount(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                              ( String.IsNullOrEmpty(filters.Result) || entity.Result.Equals(filters.Result) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId);
        }

    }
}
