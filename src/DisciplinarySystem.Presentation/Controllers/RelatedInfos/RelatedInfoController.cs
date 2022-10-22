using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.RelatedInfos.Interfaces;
using DisciplinarySystem.Application.RelatedInfos.ViewModels;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.Presentation.Controllers.RelatedInfos.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.RelatedInfos
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class RelatedInfoController : Controller
    {
        private readonly IRelatedInfoService _infoService;
        private readonly IWebHostEnvironment _hostEnv;

        private static RelatedInfoFilter _filters = new RelatedInfoFilter();

        public RelatedInfoController ( IRelatedInfoService infoService , IWebHostEnvironment hostEnv )
        {
            _infoService = infoService;
            _hostEnv = hostEnv;
        }

        public async Task<IActionResult> Index ( RelatedInfoFilter filters )
        {
            _filters = filters;
            filters.CreateDate = filters.CreateDate.ToMiladi();
            var vm = new GetAllRelatedInfos
            {
                RelatedInfos = await GetFilteredInfos(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var info = await _infoService.GetByIdAsync(id);
            if ( info == null )
            {
                TempData[SD.Warning] = "اطلاعات مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(info);
        }

        public IActionResult Create ( long caseId )
        {
            var vm = new CreateRelatedInfo
            {
                CaseId = caseId ,
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateRelatedInfo command )
        {
            if ( !ModelState.IsValid )
                return View(command);

            var files = HttpContext.Request.Form.Files;
            await _infoService.CreateAsync(command , files);
            TempData[SD.Success] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<IActionResult> Update ( Guid id )
        {
            var info = await _infoService.GetByIdAsync(id);
            if ( info == null )
            {
                TempData[SD.Warning] = "اطلاعات مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var vm = UpdateRelatedInfo.Create(info);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateRelatedInfo command )
        {
            if ( !ModelState.IsValid )
            {
                command.CurrentDocuments = ( List<RelatedInfoDocument>? ) await _infoService.GetDocumentsAsync(command.Id);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _infoService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<JsonResult> Remove ( Guid id ) => Json(new { Success = await _infoService.RemoveAsync(id) });

        public async Task<JsonResult> RemoveFile ( Guid id ) => Json(new { Success = await _infoService.RemoveFileAsync(id) });


        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.RelatedInfoDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        private async Task<IEnumerable<RelatedInfoDetails>> GetFilteredInfos ( RelatedInfoFilter filters )
        {
            return await _infoService.ListAsync(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId ,
                            skip: filters.Skip ,
                            take: filters.Take);
        }

        private int GetFilteredCount ( RelatedInfoFilter filters )
        {
            return _infoService.GetCount(
                    entity => ( String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject) ) &&
                            ( filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate) ) &&
                            filters.CaseId == entity.CaseId);
        }

    }
}
