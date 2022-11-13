using DisciplinarySystem.Application.Cases.Interfaces;
using DisciplinarySystem.Application.Complaints.Helpers;
using DisciplinarySystem.Application.Complaints.Interfaces;
using DisciplinarySystem.Application.Contracts.Interfaces;
using DisciplinarySystem.Application.Epistles.Interfaces;
using DisciplinarySystem.Application.FinalVotes.Interfaces;
using DisciplinarySystem.Application.Helpers;
using DisciplinarySystem.Application.Meetings.Interfaces;
using DisciplinarySystem.Application.Verdicts.Interfaces;
using DisciplinarySystem.Domain.Complaints.Enums;
using DisciplinarySystem.Domain.DisciplinaryCase.Cases.Enums;
using DisciplinarySystem.Presentation.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers
{
    [Authorize(Roles = "Managment,Admin")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ICaseService _caseService;
        private readonly IMeetingService _meetService;
        private readonly IComplaintService _complaintService;
        private readonly IEpistleService _epistleService;
        private readonly IFinalVoteService _finalVoteService;
        private readonly IVerdictService _verdictService;
        private readonly IPositionAPI _positionApi;

        public HomeController ( ILogger<HomeController> logger , ICaseService caseService , IMeetingService meetService , IComplaintService complaintService , IEpistleService epistleService , IFinalVoteService finalVoteService , IVerdictService verdictService , IPositionAPI positionApi )
        {
            _logger = logger;
            _caseService = caseService;
            _meetService = meetService;
            _complaintService = complaintService;
            _epistleService = epistleService;
            _finalVoteService = finalVoteService;
            _verdictService = verdictService;
            _positionApi = positionApi;
        }

        public async Task<IActionResult> Index ()
        {
            var todayMeetings = await _meetService.ListAsync(u => u.HoldingTime.From.Date.Equals(DateTime.Now.Date));
            var tomorrowMeetings = await _meetService.ListAsync(u => u.HoldingTime.From.Date.Equals(DateTime.Now.AddDays(1).Date));
            var yearBegin = DateTime.Now.ShamsiYearBegin();
            var yearEnd = DateTime.Now.ShamsiYearEnd();


            var model = new DashboradVM()
            {
                CurrentCasesCount = _caseService.GetCount(u => u.Status < CaseStatus.FinalVote) ,
                EndCasesCount = _caseService.GetCount(u => u.Status >= CaseStatus.FinalVote) ,
                MeetingsCount = _meetService.GetCount(u => u.HoldingTime.From.Date >= yearBegin.Date && u.HoldingTime.From.Date <= yearEnd.Date) ,
                TodaysMeetings = new List<(string From, string To)>() ,
                TomorrowMeetings = new List<(string From, string To)>()
            };

            todayMeetings.ToList().ForEach(meet => model.TodaysMeetings.Add(new(
                meet.HoldingTime.From.TimeOfDay.ToString().Substring(0 , 5) ,
                meet.HoldingTime.To.TimeOfDay.ToString().Substring(0 , 5))));

            tomorrowMeetings.ToList().ForEach(meet => model.TomorrowMeetings.Add(new(
                meet.HoldingTime.From.TimeOfDay.ToString().Substring(0 , 5) ,
                meet.HoldingTime.To.TimeOfDay.ToString().Substring(0 , 5))));

            return View(model);
        }

        public JsonResult GetComplaintsInLastFourYears ()
        {
            List<TotalComplaintsCountInYear> complaints = new List<TotalComplaintsCountInYear>();
            for ( int i = 0 ; i < 4 ; i++ )
            {
                var currentDate = DateTime.Now.AddYears(-i);
                var complaintsNumber = _complaintService.GetCount(u => u.CreateDate.Year.Equals(currentDate.Year));
                complaints.Add(new TotalComplaintsCountInYear { Year = currentDate.GetShamsiYear() , ComplaintsNumber = complaintsNumber });
            }

            return Json(new { complaints });
        }

        public JsonResult GetComplaintsInLast12Months ()
        {
            List<TotalComplaintsCountInMonth> complaints = new List<TotalComplaintsCountInMonth>();

            var pc = new PersianCalendar();

            for ( int i = 0 ; i < 12 ; i++ )
            {
                int year = DateTime.Now.AddMonths(-i).GetShamsiYear();
                int month = DateTime.Now.AddMonths(-i).GetShamsiMonth();
                var beginDayInMonth = new DateTime(year , month , 1 , pc);
                var lastDayInMonth = DateTime.Now.AddMonths(-i).GetTheLastDateOfTheMonth();


                complaints.Add(new TotalComplaintsCountInMonth
                {
                    Month = DateTime.Now.AddMonths(-i).GetShamsiMonthName() ,
                    ComplaintsNumber = _complaintService.GetCount(u => u.CreateDate.Date >= beginDayInMonth.Date &&
                            u.CreateDate.Date <= lastDayInMonth.Date)
                });
            }

            return Json(complaints);
        }

        public JsonResult GetDisciplinaryInfo ()
        {
            var complaintsNumber = _complaintService.GetCount();
            var casesNumber = _caseService.GetCount();
            var epistleNumber = _epistleService.GetCount();

            return Json(new { Complaints = complaintsNumber , Cases = casesNumber , Epistles = epistleNumber });
        }

        public async Task<JsonResult> GetVerdictsCountInUsePrimaryVotes ()
        {
            var verdicts = await _verdictService.GetFinalVoteCountAsync();
            return Json(verdicts);
        }

        public JsonResult GettingAllKindsOfComplaintResults ()
        {
            var info = new List<ComplaintsResultChart>();
            info.Add(new ComplaintsResultChart
            {
                Result = ComplaintResult.Archive.ToPersian() ,
                Count = _complaintService.GetCount(u => u.Result.Equals(ComplaintResult.Archive))
            });
            info.Add(new ComplaintsResultChart
            {
                Result = ComplaintResult.SeeCase.ToPersian() ,
                Count = _complaintService.GetCount(u => u.Result.Equals(ComplaintResult.SeeCase))
            });

            return Json(info);
        }

        public JsonResult GettingCaseVotesPart ()
        {
            List<CaseVotePart> info = new List<CaseVotePart>();
            info.Add(new CaseVotePart
            {
                Part = "رای اولیه" ,
                Count = _caseService.GetCount(u => u.Status.Equals(CaseStatus.PrimaryVote))
            });
            info.Add(new CaseVotePart
            {
                Part = "حکم نهایی" ,
                Count = _caseService.GetCount(u => u.Status.Equals(CaseStatus.FinalVote))
            });
            info.Add(new CaseVotePart
            {
                Part = "حکم کمیته مرکزی" ,
                Count = _caseService.GetCount(u => u.Status.Equals(CaseStatus.CentralCommitteeVote))
            });

            return Json(info);
        }

    }
}