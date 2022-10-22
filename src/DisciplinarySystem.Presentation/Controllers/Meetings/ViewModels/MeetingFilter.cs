using DisciplinarySystem.Application.Helpers;
using System.Globalization;

namespace DisciplinarySystem.Presentation.Controllers.Meetings.ViewModels
{
    public class MeetingFilter
    {
        public DateTime MeetingsDate { get; set; } = DateTime.Now;

        public String Dir { get; set; }

        public void RoundMeetingsDate()
        {
            PersianCalendar pc = new PersianCalendar();
            if (MeetingsDate == default)
                MeetingsDate = DateTime.Now;


            MeetingsDate = new DateTime(pc.GetYear(MeetingsDate), pc.GetMonth(MeetingsDate), 1);
            MeetingsDate = MeetingsDate.ToMiladi();
        }


        public void Move()
        {
            if (!String.IsNullOrEmpty(Dir) && Dir.ToLower().Equals("next"))
                Next();
            else if (!String.IsNullOrEmpty(Dir) && Dir.ToLower().Equals("prev"))
                Prev();
        }

        private void Next()
        {
            if (String.IsNullOrEmpty(Dir) || !Dir.ToLower().Equals("next"))
                return;

            var pc = new PersianCalendar();
            MeetingsDate = pc.AddMonths(MeetingsDate, 1);
        }

        private void Prev()
        {
            if (String.IsNullOrEmpty(Dir) || !Dir.ToLower().Equals("prev"))
                return;

            var pc = new PersianCalendar();
            MeetingsDate = pc.AddMonths(MeetingsDate, -1);
        }
    }
}
