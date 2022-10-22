using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.PrimaryVotes.Interfaces;
using DisciplinarySystem.Application.PrimaryVotes.ViewModels;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Domain.PrimaryVotes;
using DisciplinarySystem.Presentation.Controllers.PrimaryVotes.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.PrimaryVotes
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class PrimaryVoteController : Controller
    {
        private readonly IPrimaryVoteService _prvService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IViolationService _violationService;

        private static PrimaryVoteFilter _filters = new PrimaryVoteFilter();

        public PrimaryVoteController ( IPrimaryVoteService prvService , IWebHostEnvironment hostEnv , IViolationService violationService )
        {
            _prvService = prvService;
            _hostEnv = hostEnv;
            _violationService = violationService;
        }

        public async Task<IActionResult> Index ( PrimaryVoteFilter filters )
        {
            _filters = filters;
            filters.CreateTime = filters.CreateTime.ToMiladi();
            filters.Verdicts = await _prvService.GetSelectedVotesAsync();
            filters.Violatons = await _violationService.GetSelectedListAsync();

            var vm = new GetAllPrimaryVotes
            {
                PrimaryVotes = await GetFilteredPrimaryVotes(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _prvService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "رای وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            return View(entity);
        }

        public async Task<IActionResult> Create ( long caseId )
        {
            var command = new CreatePrimaryVote
            {
                CaseId = caseId ,
                Verdicts = await _prvService.GetSelectedVotesAsync() ,
                Violations = await _violationService.GetSelectedListAsync()
            };
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreatePrimaryVote command )
        {
            if ( !ModelState.IsValid )
            {
                command.Verdicts = await _prvService.GetSelectedVotesAsync();
                command.Violations = await _violationService.GetSelectedListAsync();
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _prvService.CreateAsync(command , files);
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Update ( Guid id )
        {
            var entity = await _prvService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "رای وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var command = UpdatePrimaryVote.Create(entity);
            command.Violations = await _violationService.GetSelectedListAsync();
            command.Verdicts = await _prvService.GetSelectedVotesAsync();
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdatePrimaryVote command )
        {
            if ( !ModelState.IsValid )
            {
                command.Verdicts = await _prvService.GetSelectedVotesAsync();
                command.Violations = await _violationService.GetSelectedListAsync();
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _prvService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id , long caseId )
        {
            var res = await _prvService.RemoveAsync(id , caseId);
            return Json(new { Success = res });

        }
        public async Task<JsonResult> RemoveFile ( Guid id ) => Json(new { Success = await _prvService.RemoveFileAsync(id) });

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.PrimaryVoteDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        private async Task<IEnumerable<PrimaryVote>> GetFilteredPrimaryVotes ( PrimaryVoteFilter filters )
        {
            return await _prvService.ListAsync(
                    filter: entity => ( !filters.ViolationId.HasValue || entity.ViolationId.Equals(filters.ViolationId.Value) ) &&
                    ( !filters.VerdictId.HasValue || entity.VerdictId.Equals(filters.VerdictId.Value) ) &&
                    ( filters.CreateTime == default || entity.CreateTime.Date.Equals(filters.CreateTime) ) &&
                    entity.Violation.CaseId == filters.CaseId ,
                    skip: filters.Skip ,
                    take: filters.Take
                );
        }
        private int GetFilteredCount ( PrimaryVoteFilter filters )
        {
            return _prvService.GetCount(
                    filter: entity => ( !filters.ViolationId.HasValue || entity.ViolationId.Equals(filters.ViolationId.Value) ) &&
                    ( !filters.VerdictId.HasValue || entity.VerdictId.Equals(filters.VerdictId.Value) ) &&
                    ( filters.CreateTime == default || entity.CreateTime.Date.Equals(filters.CreateTime) ) &&
                    entity.Violation.CaseId == filters.CaseId
                );
        }
    }
}
