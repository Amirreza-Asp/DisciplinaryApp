using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Application.Defences.Interfaces;
using DisciplinarySystem.Application.Defences.ViewModels;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.Presentation.Controllers.Defences.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Defences
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class DefenceController : Controller
    {
        private readonly IDefenceService _defService;
        private readonly IComplainingService _cmpService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IPrimaryVoteService _primaryVoteService;

        private static DefenceFilter _filters = new DefenceFilter();

        public DefenceController(IDefenceService defService, IComplainingService cmpService, IWebHostEnvironment hostEnv, IPrimaryVoteService primaryVoteService)
        {
            _defService = defService;
            _cmpService = cmpService;
            _hostEnv = hostEnv;
            _primaryVoteService = primaryVoteService;
        }

        public async Task<IActionResult> Index(DefenceFilter filters)
        {
            _filters = filters;
            filters.CreateDate = filters.CreateDate.ToMiladi();
            var vm = new GetAllDefences
            {
                Defences = await GetFilteredDefences(filters),
                TotalCount = GetFilteredCount(filters),
                Filters = filters
            };
            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var defence = await _defService.GetByIdWithCaseAsync(id);
            if (defence == null)
            {
                TempData[SD.Warning] = "دفاعیه مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            return View(defence);
        }

        public async Task<IActionResult> Create(long caseId)
        {
            var comp = await _cmpService.GetByCaseIdAsync(caseId);
            if (comp == null)
            {
                TempData[SD.Error] = "شماره پرونده وارد شده اشتباه است";
                return RedirectToAction(nameof(Index), _filters);
            }

            var vm = new CreateDefence
            {
                CaseId = caseId,
                Complaining = CreateComplaining.Create(comp)
            };

            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDefence command)
        {
            if (!ModelState.IsValid)
                return View(command);

            var files = HttpContext.Request.Form.Files;
            await _defService.CreateAsync(command, files);
            TempData[SD.Success] = "دفاعیه با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index), _filters);
        }


        public async Task<IActionResult> Update(Guid id)
        {
            var defence = await _defService.GetByIdWithCaseAsync(id);
            if (defence == null)
            {
                TempData[SD.Warning] = "دفاعیه مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            var vm = UpdateDefence.Create(defence);
            return View(vm);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateDefence command)
        {
            if (!ModelState.IsValid)
            {
                command.CurrentDocuments = (List<DefenceDocument>?)await _defService.GetDocumentsAsync(command.Id);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _defService.UpdateAsync(command, files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index), _filters);
        }


        public async Task<JsonResult> Remove(Guid id) => Json(new { Success = await _defService.RemoveAsync(id) });

        public async Task<JsonResult> RemoveFile(Guid id) => Json(new { Success = await _defService.RemoveFileAsync(id) });


        public IActionResult Download(String file, String fileName)
        {
            string filePath = _hostEnv.WebRootPath + SD.DefenceDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", fileName);
        }


        private async Task<IEnumerable<DefenceDetails>> GetFilteredDefences(DefenceFilter filters)
        {
            return await _defService.ListAsync(
                    entity => (String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject)) &&
                            (filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate)) &&
                            filters.CaseId == entity.CaseId,
                            skip: filters.Skip,
                            take: filters.Take);
        }

        private int GetFilteredCount(DefenceFilter filters)
        {
            return _defService.GetCount(
                    entity => (String.IsNullOrEmpty(filters.Subject) || entity.Subject.Contains(filters.Subject)) &&
                            (filters.CreateDate == default || entity.CreateDate.Date.Equals(filters.CreateDate)) &&
                            filters.CaseId == entity.CaseId);
        }

    }
}
