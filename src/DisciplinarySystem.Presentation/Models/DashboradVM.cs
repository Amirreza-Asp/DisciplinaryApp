namespace DisciplinarySystem.Presentation.Models
{
    public class DashboradVM
    {
        public long CurrentCasesCount { get; set; }
        public long EndCasesCount { get; set; }
        public List<(String From, String To)> TodaysMeetings { get; set; }
        public List<(String From, String To)> TomorrowMeetings { get; set; }
        public int MeetingsCount { get; set; }

    }
}
