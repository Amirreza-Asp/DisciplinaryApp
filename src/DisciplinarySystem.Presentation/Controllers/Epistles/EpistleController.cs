using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.Epistles.Interfaces;
using DisciplinarySystem.Application.Epistles.ViewModels;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Presentation.Controllers.Epistles.ViewModels;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers.Epistles
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class EpistleController : Controller
    {
        private readonly IEpistleService _service;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly ICaseService _caseService;
        private readonly IRepository<User> _userRepo;
        private readonly IPositionAPI _positionApi;

        private static EpistleFilter _filters = new EpistleFilter();

        public EpistleController ( IEpistleService service , IWebHostEnvironment hostEnv , ICaseService caseService , IRepository<User> userRepo , IPositionAPI positionApi )
        {
            _service = service;
            _hostEnv = hostEnv;
            _caseService = caseService;
            _userRepo = userRepo;
            _positionApi = positionApi;
        }

        public async Task<IActionResult> Index ( EpistleFilter filters )
        {
            _filters = filters;
            PersianCalendar pc = new PersianCalendar();
            if ( filters.CreateDate != default )
            {
                filters.CreateDate = new DateTime(filters.CreateDate.Year , filters.CreateDate.Month , filters.CreateDate.Day , pc);
            }

            var epistlesListVM = new GetEpistlesListVM
            {
                Epistles = await GetFilteredEpistles(filters) ,
                TotalCount = GetFilteredCount(filters) ,
                Filters = filters
            };
            return View(epistlesListVM);
        }

        public async Task<IActionResult> Create ( long caseId )
        {
            var caseEntity = await _caseService.GetByIdAsync(caseId);
            if ( caseId > 0 && caseEntity == null )
            {
                TempData[SD.Error] = "پرونده مورد نظر وجود ندارد";
                return Redirect(Request.GetTypedHeaders().Referer.ToString());
            }
            else if ( caseEntity != null )
            {
                var command = new CreateEpistle { CaseId = caseId , ComplaintId = caseEntity.ComplaintId };
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                return View(command);
            }

            var dto = new CreateEpistle() { CaseId = null , ComplaintId = null };
            var position = await _positionApi.GetPositionsAsync();
            dto.Positions = position.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
            return View(dto);
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateEpistle command )
        {
            if ( !ModelState.IsValid )
            {
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                return View(command);
            }

            if ( command.CaseId.HasValue && !_service.HasCase(command.CaseId.Value) )
            {
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                TempData[SD.Error] = "شماره پرونده وارد شده اشتباه است";
                return View(command);
            }

            if ( command.ComplaintId.HasValue && !_service.HasComplaint(command.ComplaintId.Value) )
            {
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                TempData[SD.Error] = "شماره شکایت وارد شده اشتباه است";
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;

            await _service.CreateAsync(command , files);
            TempData[SD.Success] = "نامه با موفقیت اضافه شد";
            return RedirectToAction(nameof(Index) , _filters);
        }


        public async Task<JsonResult> Remove ( long id )
        {
            if ( await _service.RemoveAsync(id) )
                return Json(new { Success = true });
            return Json(new { Success = false });
        }

        public async Task<IActionResult> Update ( long id , bool withSteps )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "شماره نامه وارد شده اشتباه است";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var updateEpistle = UpdateEpistle.Create(entity);
            updateEpistle.WithSteps = withSteps;
            var positions = await _positionApi.GetPositionsAsync();
            updateEpistle.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
            return View(updateEpistle);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateEpistle command )
        {
            if ( !ModelState.IsValid )
            {
                command.CurrentDocuments = await _service.GetCurrentDocumentsAsync(command.Id);
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                command.Reciver = String.Empty;
                command.Sender = String.Empty;
                return View(command);
            }


            if ( command.CaseId.HasValue && !_service.HasCase(command.CaseId.Value) )
            {
                TempData[SD.Error] = "شماره پرونده وارد شده اشتباه است";
                command.CurrentDocuments = await _service.GetCurrentDocumentsAsync(command.Id);
                var positions = await _positionApi.GetPositionsAsync();
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                command.Reciver = String.Empty;
                command.Sender = String.Empty;
                return View(command);
            }

            if ( command.ComplaintId.HasValue && !_service.HasComplaint(command.ComplaintId.Value) )
            {
                TempData[SD.Error] = "شماره شکایت وارد شده اشتباه است";
                command.CurrentDocuments = await _service.GetCurrentDocumentsAsync(command.Id);
                var positions = await _positionApi.GetPositionsAsync();
                command.Reciver = String.Empty;
                command.Sender = String.Empty;
                command.Positions = positions.Select(u => new SelectListItem { Text = u.Title , Value = u.Title });
                return View(command);
            }

            var files = HttpContext.Request.Form.Files;
            await _service.UpdateAsync(command , files);
            TempData[SD.Info] = "ویرایش نامه با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filters);
        }

        public async Task<IActionResult> Details ( long id , bool withSteps )
        {
            var entity = await _service.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Error] = "شماره نامه وارد شده اشتباه است";
                return RedirectToAction(nameof(Index) , _filters);
            }

            var vm = new GetEpistleDetailsVM { Epistle = entity , WithSteps = withSteps };

            return View(vm);
        }

        public async Task<JsonResult> RemoveFile ( Guid id )
        {
            if ( await _service.RemoveFileAsync(id) )
                return Json(new { Success = true });
            return Json(new { Success = false });
        }

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.EpistleDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }


        public async Task<JsonResult> GetPositions ( String id )
        {
            var users = await _positionApi.GetUsersAsync(id);
            if ( users != null )
            {
                var data = users.Select(u => new SelectListItem { Text = u.Name , Value = u.NationalCode });
                return Json(new { Exists = true , Data = data });
            }
            return Json(new { Success = false });
        }

        #region Utilities
        private async Task<IEnumerable<GetEpistle>> GetFilteredEpistles ( EpistleFilter filters )
        {
            return await _service.GetAllAsync(
                        filter: u =>
                                ( u.Reciver.Contains(filters.Sender_Reciver) ||
                                    u.Sender.Contains(filters.Sender_Reciver) ||
                                    String.IsNullOrEmpty(filters.Sender_Reciver) ) &&
                                ( u.Subject.Contains(filters.Subject) ||
                                    String.IsNullOrEmpty(filters.Subject) ) &&
                                ( u.Type.Contains(filters.Type) ||
                                    String.IsNullOrEmpty(filters.Type) ) &&
                                ( u.Id.Equals(filters.Id) || filters.Id <= 0 ) &&
                                ( u.CaseId.Equals(filters.CaseId) || filters.CaseId <= 0 ) &&
                                ( u.ComplaintId.Equals(filters.ComplaintId) || filters.ComplaintId <= 0 ) &&
                                ( filters.CreateDate == default ||
                                    u.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                    u.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                    u.CreateDate.Day.Equals(filters.CreateDate.Day) ) ,
                                skip: filters.Skip ,
                                take: filters.Take);
        }

        private int GetFilteredCount ( EpistleFilter filters )
        {
            return _service.GetCount(
                        filter: u =>
                                ( u.Reciver.Contains(filters.Sender_Reciver) ||
                                    u.Sender.Contains(filters.Sender_Reciver) ||
                                    String.IsNullOrEmpty(filters.Sender_Reciver) ) &&
                                ( u.Subject.Contains(filters.Subject) ||
                                    String.IsNullOrEmpty(filters.Subject) ) &&
                                ( u.Type.Contains(filters.Type) ||
                                    String.IsNullOrEmpty(filters.Type) ) &&
                                ( u.Id.Equals(filters.Id) || filters.Id <= 0 ) &&
                                ( u.CaseId.Equals(filters.CaseId) || filters.CaseId <= 0 ) &&
                                ( u.ComplaintId.Equals(filters.ComplaintId) || filters.ComplaintId <= 0 ) &&
                                ( filters.CreateDate == default ||
                                    u.CreateDate.Year.Equals(filters.CreateDate.Year) &&
                                    u.CreateDate.Month.Equals(filters.CreateDate.Month) &&
                                    u.CreateDate.Day.Equals(filters.CreateDate.Day) ));
        }
        #endregion
    }
}
