using DisciplinarySystem.Domain.Meetings;

namespace DisciplinarySystem.Presentation.Controllers.Meetings.ViewModels
{
    public class GetAllMeetings
    {
        public IEnumerable<Meeting> Meetings { get; set; }
        public MeetingFilter MeetingFilter { get; set; }
    }
}
