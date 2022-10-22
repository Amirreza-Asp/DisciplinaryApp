using DisciplinarySystem.Domain.Users;

namespace DisciplinarySystem.Domain.Meetings
{
    public class MeetingUsers
    {
        public MeetingUsers(Guid userId, Guid meetingId)
        {
            UserId = Guard.Against.Default(userId);
            MeetingId = Guard.Against.Default(meetingId);
        }

        private MeetingUsers() { }

        public Guid UserId { get; private set; }
        public Guid MeetingId { get; private set; }

        public User User { get; private set; }
        public Meeting Meeting { get; private set; }
    }
}
