using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Application.FinalVotes.Interfaces;
using DisciplinarySystem.Application.FinalVotes.ViewModels;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Users.Interfaces;
using DisciplinarySystem.Application.Violations.Intefaces;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Presentation.Controllers.FinalVotes.ViewModels;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class FinalVoteController : Controller
    {
        private readonly IFinalVoteService _fvoService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IUserService _userService;
        private readonly IComplainingService _comService;
        private readonly IViolationService _violatonService;

        private static FinalVoteFilter _filters = new FinalVoteFilter();

        public FinalVoteController ( IFinalVoteService fvoService , IWebHostEnvironment hostEnv , IUserService userService , IComplainingService comService , IViolationService violatonService )
        {
            _fvoService = fvoService;
            _hostEnv = hostEnv;
            _userService = userService;
            _comService = comService;
            _violatonService = violatonService;
        }

        public async Task<IActionResult> Index ( FinalVoteFilter filters )
        {
            _filters = filters;
            filters.CreateTime = filters.CreateTime.ToMiladi();
            filters.Verdicts = await _fvoService.GetSelectedVotesAsync();

            var vm = new GetAllFinalVotes
            {
                FinalVotes = await GetFilteredFinalVotes(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };

            return View(vm);
        }

        public async Task<IActionResult> Details ( Guid id )
        {
            var entity = await _fvoService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "حکم وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }


            var vm = FinalVoteDetails.Create(entity);
            vm.Users = await _userService.GetUsersNameAsync();
            vm.Complaining = CreateComplaining.Create(await _comService.GetByCaseIdAsync(entity.Violation.CaseId));

            return View(vm);
        }

        public async Task<IActionResult> Create ( long caseId )
        {
            var complaining = await _comService.GetByCaseIdAsync(caseId);

            if ( complaining == null )
            {
                TempData[SD.Warning] = "شماره پرونده وارد شده اشتباه است";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var command = new CreateFinalVote
            {
                CaseId = caseId ,
                Violations = await _violatonService.GetSelectedListAsync() ,
                Verdicts = await _fvoService.GetSelectedVotesAsync() ,
                Users = await _userService.GetUsersNameAsync() ,
                Complaining = CreateComplaining.Create(complaining)
            };
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateFinalVote command )
        {
            if ( !ModelState.IsValid )
            {
                command.Verdicts = await _fvoService.GetSelectedVotesAsync();
                command.Violations = await _violatonService.GetSelectedListAsync();
                command.Users = await _userService.GetUsersNameAsync();
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _fvoService.CreateAsync(command , files);
            TempData[SD.Success] = "اطلاعات با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Update ( Guid id )
        {
            var entity = await _fvoService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "حکم وارد شده وجود ندارد";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var command = UpdateFinalVote.Create(entity);
            command.Verdicts = await _fvoService.GetSelectedVotesAsync();
            command.Users = await _userService.GetUsersNameAsync();
            command.Violations = await _violatonService.GetSelectedListAsync();
            command.Complaining = CreateComplaining.Create(await _comService.GetByCaseIdAsync(entity.Violation.CaseId));
            return View(command);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateFinalVote command )
        {
            if ( !ModelState.IsValid )
            {
                command.Verdicts = await _fvoService.GetSelectedVotesAsync();
                command.Users = await _userService.GetUsersNameAsync();
                command.Violations = await _violatonService.GetSelectedListAsync();
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _fvoService.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<JsonResult> Remove ( Guid id , long caseId ) => Json(new { Success = await _fvoService.RemoveAsync(id , caseId) });
        public async Task<JsonResult> RemoveFile ( Guid id ) => Json(new { Success = await _fvoService.RemoveFileAsync(id) });

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.FinalVoteDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        private async Task<IEnumerable<FinalVote>> GetFilteredFinalVotes ( FinalVoteFilter filters )
        {
            return await _fvoService.ListAsync(
                    filter: entity => ( !filters.ViolationId.HasValue || entity.ViolationId.Equals(filters.ViolationId.Value) ) &&
                    ( !filters.VerdictId.HasValue || entity.VerdictId.Equals(filters.VerdictId.Value) ) &&
                    ( filters.CreateTime == default || entity.CreateTime.Date.Equals(filters.CreateTime) ) &&
                    entity.Violation.CaseId == filters.CaseId ,
                    skip: filters.Skip ,
                    take: filters.Take
                );
        }

        private int GetFilteredCount ( FinalVoteFilter filters )
        {
            return _fvoService.GetCount(
                    filter: entity => ( !filters.ViolationId.HasValue || entity.ViolationId.Equals(filters.ViolationId.Value) ) &&
                    ( !filters.VerdictId.HasValue || entity.VerdictId.Equals(filters.VerdictId.Value) ) &&
                    ( filters.CreateTime == default || entity.CreateTime.Date.Equals(filters.CreateTime) ) &&
                    entity.Violation.CaseId == filters.CaseId);
        }
    }
}
