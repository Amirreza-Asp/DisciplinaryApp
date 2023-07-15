using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.Interfaces;
using DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.ViewModels;
using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Application.Verdicts.Interfaces;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class CentralCommitteeVoteController : Controller
    {
        private readonly ICentralCommitteeVoteService _ccvService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IViolationService _violationService;
        private readonly IVerdictService _verdictService;
        private readonly IPrimaryVoteService _primaryVoteService;

        private static CommitteeVoteFilter _filters = new CommitteeVoteFilter();

        public CentralCommitteeVoteController(ICentralCommitteeVoteService ccvService, IWebHostEnvironment hostEnv, IViolationService violationService, IVerdictService verdictService, IPrimaryVoteService primaryVoteService)
        {
            _ccvService = ccvService;
            _hostEnv = hostEnv;
            _violationService = violationService;
            _verdictService = verdictService;
            _primaryVoteService = primaryVoteService;
        }

        public async Task<IActionResult> Index(CommitteeVoteFilter filters)
        {
            if (_primaryVoteService.GetByCaseIdAsync(filters.CaseId).GetAwaiter().GetResult().IsClosed)
            {
                TempData[SD.Error] = "پرونده مختومه است";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }

            _filters = filters;

            var vm = new GetAllComitteeVotes
            {
                CentralCommitteeVotes = await GetFilteredCommitteeVotes(filters),
                Filters = filters
            };

            return View(vm);
        }

        public async Task<IActionResult> Details(Guid id)
        {
            var entity = await _ccvService.GetByIdAsync(id);
            if (entity == null)
            {
                TempData[SD.Warning] = "حکم کمیته وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            return View(entity);
        }

        public async Task<IActionResult> Create(long caseId)
        {
            var command = new CreateCentralCommitteeVote
            {
                CaseId = caseId,
                Verdicts = await _verdictService.GetSelectListAsync(),
                Violations = await _violationService.GetSelectedListAsync(b => b.CaseId == caseId)
            };
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCentralCommitteeVote command)
        {
            if (!ModelState.IsValid)
            {
                command.Verdicts = await _verdictService.GetSelectListAsync();
                command.Violations = await _violationService.GetSelectedListAsync(b => b.CaseId == command.CaseId);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _ccvService.CreateAsync(command, files);
            return RedirectToAction(nameof(Index), _filters);
        }

        public async Task<IActionResult> Update(Guid id)
        {
            var entity = await _ccvService.GetByIdAsync(id);
            if (entity == null)
            {
                TempData[SD.Warning] = "حکم کمیته وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            var command = UpdateCentralCommitteeVote.Create(entity);
            command.Violations = await _violationService.GetSelectedListAsync(b => b.CaseId == _filters.CaseId);
            command.Verdicts = await _verdictService.GetSelectListAsync();
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update(UpdateCentralCommitteeVote command)
        {
            if (!ModelState.IsValid)
            {
                command.Verdicts = await _verdictService.GetSelectListAsync();
                command.Violations = await _violationService.GetSelectedListAsync(b => b.CaseId == command.CaseId);
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _ccvService.UpdateAsync(command, files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index), _filters);
        }

        public async Task<JsonResult> Remove(Guid id, long caseId) => Json(new { Success = await _ccvService.RemoveAsync(id, caseId) });
        public async Task<JsonResult> RemoveFile(Guid id) => Json(new { Success = await _ccvService.RemoveFileAsync(id) });

        public IActionResult Download(String file, String fileName)
        {
            string filePath = _hostEnv.WebRootPath + SD.CentralCommitteeVoteDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes, "application/force-download", fileName);
        }


        private async Task<IEnumerable<CentralCommitteeVote>> GetFilteredCommitteeVotes(CommitteeVoteFilter filters)
        {
            return await _ccvService.ListAsync(
                filter: entity => entity.Violation.CaseId == filters.CaseId);
        }
    }
}
