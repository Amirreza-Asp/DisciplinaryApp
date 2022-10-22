using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Complaints.Helpers;
using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Complaints.ViewModels;
using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Application.Complaints.ViewModels.Update;
using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels;
using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using GetComplaintsListVM = DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels.GetComplaintsListVM;

namespace DisciplinarySystem.Presentation.Controllers.Complaints
{
    [Authorize(Roles = $"{SD.Admin},{SD.Managment}")]
    public class ComplaintController : Controller
    {
        private readonly IComplaintService _complaintService;
        private readonly IWebHostEnvironment _hostEnv;
        private readonly IUserApi _userApi;
        private readonly ICaseService _caseService;

        private static FilterVM _filterVM = new FilterVM();

        public ComplaintController ( IComplaintService complaintService , IWebHostEnvironment hostEnv , IUserApi userApi , ICaseService caseService )
        {
            _complaintService = complaintService;
            _hostEnv = hostEnv;
            _userApi = userApi;
            _caseService = caseService;
        }

        public async Task<IActionResult> Index ( FilterVM filterVM )
        {
            filterVM = SetFilters(filterVM);
            var complintsVm = new GetComplaintsListVM
            {
                Complaints = await GetFilteredComplaints(filterVM) ,
                TotalCount = GetFilteredCount(filterVM) ,
                Filters = SetFilters(filterVM)
            };

            return View(complintsVm);
        }

        public async Task<IActionResult> Details ( long id , long caseId )
        {
            var entity = await _complaintService.GetByIdAsync(id);
            if ( entity == null )
            {
                TempData[SD.Warning] = "شکایت مورد نظر پیدا نشد";
                return RedirectToAction(nameof(Index) , _filterVM);
            }

            var vm = new ComplaintDetailsVM { Complaint = entity , CaseId = caseId };
            return View(vm);
        }

        public IActionResult Create ()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create ( CreateComplaint createComplaint )
        {
            if ( !ModelState.IsValid )
                return View(createComplaint);

            var complainingInfo = await _complaintService.GetUserAsync(createComplaint.Complaining.NationalCode);
            if ( complainingInfo == null )
            {
                TempData[SD.Error] = "متشاکی وارد شده وجود ندارد";
                return View(complainingInfo);
            }

            createComplaint.Complaining = SetComplainingInfo(createComplaint.Complaining , complainingInfo);
            var files = HttpContext.Request.Form.Files;
            await _complaintService.CreateAsync(createComplaint , files);

            TempData[SD.Success] = "شکایت با موفقیت ذخیره شد";
            return RedirectToAction(nameof(Index) , _filterVM);
        }

        public async Task<IActionResult> Update ( long id , long caseId )
        {
            var complaint = await _complaintService.GetByIdAsync(id);
            if ( complaint == null )
            {
                TempData[SD.Error] = "شکایت مورد نظر وجود ندارد";
                return RedirectToAction(nameof(Index) , _filterVM);
            }

            var updateComplaint = UpdateComplaint.CreateWith(complaint);
            updateComplaint.CaseId = caseId;
            return View(updateComplaint);
        }
        [HttpPost]
        public async Task<IActionResult> Update ( UpdateComplaint updateComplaint )
        {
            if ( !ModelState.IsValid )
            {
                updateComplaint.Results = new List<SelectListItem>
                {
                    new SelectListItem(){Text = ComplaintResult.Archive.ToPersian() , Value = ComplaintResult.Archive.ToString()},
                    new SelectListItem(){Text = ComplaintResult.Filing.ToPersian() , Value = ComplaintResult.Filing.ToString()}

                };
                return View(updateComplaint);
            }


            var files = HttpContext.Request.Form.Files;
            await _complaintService.UpdateAsync(updateComplaint , files);
            TempData[SD.Info] = "ویرایش شکایت با موفقیت انجام شد";
            return RedirectToAction(nameof(Index) , _filterVM);

        }

        public async Task<JsonResult> Remove ( long id )
        {
            if ( await _complaintService.RemoveAsync(id) )
                return Json(new { Success = true , Message = "ایتم با موفقیت حذف شد" });
            return Json(new { Success = false , Message = "حذف با شکست مواجه شد" });
        }

        public async Task<JsonResult> RemoveFile ( Guid id )
        {
            if ( await _complaintService.RemoveFileAsync(id) )
                return Json(new { Success = true , Message = "حذف با موفقیت انجام شد" });
            return Json(new { Success = false , Message = "حذف با شکست مواجه شد" });
        }

        public IActionResult Download ( String file , String fileName )
        {
            string filePath = _hostEnv.WebRootPath + SD.ComplaintDocumentPath + file;
            byte[] fileBytes = System.IO.File.ReadAllBytes(filePath);
            return File(fileBytes , "application/force-download" , fileName);
        }

        public async Task<JsonResult> GetUserInfo ( String id )
        {
            var user = await _userApi.GetUserAsync(id);
            if ( user != null )
                return Json(new
                {
                    Exists = true ,
                    Data = new
                    {
                        FullName = $"{user.Name} {user.Lastname}" ,
                        StudentNumber = user.StudentNumber ,
                        College = user.Trend ,
                        Grade = user.Grade ,
                        Father = user.Fathername
                    }
                });
            return Json(new { Exists = false });
        }

        #region Utilities
        private CreateComplaining SetComplainingInfo ( CreateComplaining comp , UserInfo user )
        {
            comp.StudentNumber = user.StudentNumber.ToString();
            comp.FullName = user.Name + " " + user.Lastname;
            comp.EducationGroup = user.Major;
            comp.College = user.Trend;
            comp.Grade = user.Grade;
            comp.Father = user.Fathername;
            return comp;
        }

        private async Task<IEnumerable<GetComplaint>> GetFilteredComplaints ( FilterVM filterVM )
        {
            return await _complaintService.GetAllAsync(
                        filter: u =>
                                ( u.Complaining.FullName.Contains(filterVM.FullName) ||
                                u.Plaintiff.FullName.Contains(filterVM.FullName) ) &&
                                u.Complaining.Grade.Contains(filterVM.Grade) &&
                                u.Complaining.EducationalGroup.Contains(filterVM.EducationalGroup) &&
                                u.Complaining.NationalCode.Value.Contains(filterVM.NationalCode) &&
                                u.Complaining.StudentNumber.Value.Contains(filterVM.StudentNumber) &&
                                u.Complaining.College.Contains(filterVM.College) &&
                                ( filterVM.CaseId <= 0 || u.Case.Id == filterVM.CaseId ) ,
                                take: filterVM.Take ,
                                skip: filterVM.Skip);
        }

        private int GetFilteredCount ( FilterVM filterVM )
        {
            return _complaintService.GetCount(filter: u =>
                                ( u.Complaining.FullName.Contains(filterVM.FullName) ||
                                u.Plaintiff.FullName.Contains(filterVM.FullName) ) &&
                                u.Complaining.Grade.Contains(filterVM.Grade) &&
                                u.Complaining.EducationalGroup.Contains(filterVM.EducationalGroup) &&
                                u.Complaining.NationalCode.Value.Contains(filterVM.NationalCode) &&
                                u.Complaining.StudentNumber.Value.Contains(filterVM.StudentNumber) &&
                                u.Complaining.College.Contains(filterVM.College) &&
                                ( filterVM.CaseId <= 0 || u.Case.Id == filterVM.CaseId )
                              );
        }

        private FilterVM SetFilters ( FilterVM filters )
        {
            filters.FullName = String.IsNullOrEmpty(filters.FullName) ? "" : filters.FullName;
            filters.College = String.IsNullOrEmpty(filters.College) ? "" : filters.College;
            filters.Grade = String.IsNullOrEmpty(filters.Grade) ? "" : filters.Grade;
            filters.EducationalGroup = String.IsNullOrEmpty(filters.EducationalGroup) ? "" : filters.EducationalGroup;
            filters.NationalCode = String.IsNullOrEmpty(filters.NationalCode) ? "" : filters.NationalCode;
            filters.StudentNumber = String.IsNullOrEmpty(filters.StudentNumber) ? "" : filters.StudentNumber;
            _filterVM = filters;
            return filters;
        }
        #endregion
    }
}
