using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Cases.ViewModels;
using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Presentation.Controllers.Cases.ViewModels;
using DisciplinarySystem.Presentation.Controllers.Complaints;
using DisciplinarySystem.SharedKernel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DisciplinarySystem.Presentation.Controllers.Cases
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class CaseController : Controller
    {
        private readonly ICaseService _caseService;
        private readonly IComplaintService _complaintService;

        private static CaseFilters _filters = new CaseFilters();

        public CaseController(ICaseService caseService, IComplaintService complaintService)
        {
            _caseService = caseService;
            _complaintService = complaintService;
        }

        public async Task<IActionResult> Index(CaseFilters filters)
        {
            filters.StatusList = filters.GetStatusList();
            var entities = await GetFilteredComplaints(filters);
            _filters = filters;

            var caseVM = new GetCasesListVM
            {
                Cases = entities.ToList(),
                TotalCount = (int)GetFilteredCount(filters),
                Filters = filters
            };
            return View(caseVM);
        }

        public IActionResult Inquiry()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Inquiry(CaseInquiry command)
        {
            if (String.IsNullOrEmpty(command.NationalCode) && !command.CaseId.HasValue)
            {
                TempData[SD.Warning] = "هیچ فیلترینگی صورت نگرفته!!!";
                return View(command);
            }

            var filters = new CaseFilters
            {
                NationalCode = command.NationalCode,
                Id = command.CaseId.HasValue ? command.CaseId.Value : 0
            };
            return RedirectToAction(nameof(Index), filters);
        }

        public async Task<IActionResult> FullInformation(long caseId)
        {
            var caseEntity = await _caseService.FullInformationAsync(caseId);

            if (caseEntity == null)
            {
                TempData[SD.Error] = "پرونده مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index), _filters);
            }

            return View(caseEntity);
        }

        public async Task<JsonResult> GetInfo(long id)
        {
            var complaint = await _complaintService.GetByIdAsync(id);
            if (complaint == null)
            {
                return Json(new { Exists = false });
            }

            return Json(new
            {
                Exists = true,
                Data = new
                {
                    ComplainigSN = complaint.Complaining.StudentNumber.Value,
                    ComplainigNC = complaint.Complaining.NationalCode.Value,
                    ComplainigFN = complaint.Complaining.FullName,
                    ComplainingCC = complaint.Complaining.College,
                    ComplainingEG = complaint.Complaining.EducationalGroup,
                    ComplainingGrade = complaint.Complaining.Grade,
                    PlaintiffFN = complaint.Plaintiff.FullName,
                    plaintiffNC = complaint.Plaintiff.NationalCode.Value,
                    plaintiffPN = complaint.Plaintiff.PhoneNumber.Value,
                }
            });
        }

        public async Task<IActionResult> Create(long complaintId)
        {
            var complaint = await _complaintService.GetByIdAsync(complaintId);

            CreateCase caseVM = new CreateCase();
            if (complaint != null)
                caseVM = CreateCase.Create(complaint);

            return View(caseVM);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCase createCase)
        {
            if (!ModelState.IsValid)
            {
                return View(createCase);
            }

            var complaint = await _complaintService.GetByIdAsync(createCase.ComplaintId);
            if (complaint == null)
            {
                TempData[SD.Error] = "شماره شکایت وارد شده اشتباه است";
                return View(createCase);
            }


            var caseEntity = await _caseService.GetByComplaintIdAsync(createCase.ComplaintId);
            if (caseEntity != null)
            {
                TempData[SD.Warning] = $"پرونده ای با شماره {caseEntity.Id} برای شکایت وارد شده وجود دارد";
                return View(createCase);
            }

            if (await _caseService.GetByIdAsync(createCase.Id) != null)
            {
                TempData[SD.Error] = "شماره پرونده وارد شده تکراری است";
                return View(createCase);
            }


            await _caseService.CreateAsync(createCase);
            TempData[SD.Success] = "پرونده با موفقیت ثبت شد";
            return RedirectToAction(nameof(Index), _filters);
        }

        public async Task<IActionResult> Details(long id, bool onlySee)
        {
            var entity = await _caseService.GetByIdAsync(id);
            if (entity == null)
            {
                TempData[SD.Error] = "شماره پرونده وارد شده اشتباه است";
                return RedirectToAction(nameof(Index), _filters);
            }
            var vm = new CaseDetails
            {
                Case = entity,
                OnlySee = onlySee
            };
            return View(vm);
        }

        public async Task<IActionResult> DetailsByComplaintId(long complaintId, bool onlySee)
        {
            var entity = await _caseService.GetByComplaintIdAsync(complaintId);
            if (entity == null)
            {
                TempData[SD.Error] = "پرونده ای برای شماره شکایت وارد شده وجود ندارد";
                return RedirectToAction(nameof(ComplaintController.Index), nameof(ComplaintController));
            }

            var vm = new CaseDetails
            {
                Case = entity,
                OnlySee = onlySee
            };
            return View(nameof(Details), vm);
        }

        public async Task<JsonResult> Remove(long id)
        {
            if (await _caseService.RemoveAsync(id))
                return Json(new { Success = true });
            return Json(new { Success = false });
        }

        #region Utilities
        private async Task<IEnumerable<GetCases>> GetFilteredComplaints(CaseFilters filters)
        {
            return await _caseService.GetAllAsync(
                        filter: u =>
                                (u.Id == filters.Id || filters.Id <= 0) &&
                                (u.Complaint.Complaining.NationalCode.Value.Equals(filters.NationalCode) ||
                                        String.IsNullOrEmpty(filters.NationalCode)) &&
                                (u.Complaint.Complaining.EducationalGroup.Equals(filters.EducationalGroup) ||
                                        String.IsNullOrEmpty(filters.EducationalGroup)) &&
                                (u.Complaint.Complaining.Grade.Equals(filters.Grade) ||
                                        String.IsNullOrEmpty(filters.Grade)) &&
                                (u.Complaint.Complaining.StudentNumber.Value.Equals(filters.StudentNumber) ||
                                        String.IsNullOrEmpty(filters.StudentNumber)) &&
                                (u.Complaint.Complaining.FullName.Equals(filters.Name) ||
                                        String.IsNullOrEmpty(filters.Name)) &&
                                (u.Complaint.Complaining.College.Equals(filters.College) ||
                                        String.IsNullOrEmpty(filters.College)) &&
                                ((int)u.Status == filters.Status || filters.Status <= 0),
                                skip: filters.Skip,
                                take: filters.Take);
        }

        private long GetFilteredCount(CaseFilters filters)
        {
            return _caseService.GetCount(filter: u =>
                                (u.Id == filters.Id || filters.Id <= 0) &&
                                (u.Complaint.Complaining.NationalCode.Value.Equals(filters.NationalCode) ||
                                        String.IsNullOrEmpty(filters.NationalCode)) &&
                                (u.Complaint.Complaining.EducationalGroup.Equals(filters.EducationalGroup) ||
                                        String.IsNullOrEmpty(filters.EducationalGroup)) &&
                                (u.Complaint.Complaining.Grade.Equals(filters.Grade) ||
                                        String.IsNullOrEmpty(filters.Grade)) &&
                                (u.Complaint.Complaining.StudentNumber.Value.Equals(filters.StudentNumber) ||
                                        String.IsNullOrEmpty(filters.StudentNumber)) &&
                                (u.Complaint.Complaining.FullName.Equals(filters.Name) ||
                                        String.IsNullOrEmpty(filters.Name)) &&
                                (u.Complaint.Complaining.College.Equals(filters.College) ||
                                        String.IsNullOrEmpty(filters.College)) &&
                                ((int)u.Status == filters.Status || filters.Status <= 0));
        }

        #endregion
    }
}
