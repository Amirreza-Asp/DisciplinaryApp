using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Meetings.ViewModels
{
    public class CreateMeeting
    {

        [Required(ErrorMessage = "عنوان جلسه را وارد کنید")]
        public String Title { get; set; }

        [Required(ErrorMessage = "زمان شروع جلسه را مشخص کنید")]
        public String Start { get; set; }

        [Required(ErrorMessage = "زمان اتمام جلسه را مشخص کنید")]
        public String End { get; set; }

        public DateTime MeetingDate { get; set; }

        public String? Description { get; set; }

        [Required(ErrorMessage = "افراد حاضر در جلسه را مشخص کنید")]
        public String InvitedUsers { get; set; }


        public IEnumerable<SelectListItem>? Users { get; set; }

        public TimeOnly GetStartTime()
        {
            if (String.IsNullOrEmpty(Start) || Start.Split(':').Length < 2)
                return new TimeOnly();

            int hour = int.Parse(Start.Split(':')[0]);
            int min = int.Parse(Start.Split(':')[1]);

            return new TimeOnly(hour, min);
        }
        public TimeOnly GetEndTime()
        {
            if (String.IsNullOrEmpty(End) || End.Split(':').Length < 2)
                return new TimeOnly();

            int hour = int.Parse(End.Split(':')[0]);
            int min = int.Parse(End.Split(':')[1]);

            return new TimeOnly(hour, min);
        }
    }
}
