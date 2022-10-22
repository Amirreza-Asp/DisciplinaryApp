namespace DisciplinarySystem.Domain.Meetings
{
    public class Meeting : BaseEntity<Guid>
    {
        public Meeting(string title, DateTimeRange holdingTime, string? description)
        {
            Id = Guid.NewGuid();
            CreateDate = DateTime.Now;
            HoldingTime = holdingTime;
            Description = description;
            Title = Guard.Against.NullOrEmpty(title);
        }

        private Meeting()
        {
        }

        public String Title { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTimeRange HoldingTime { get; private set; }
        public String? Description { get; private set; }

        public ICollection<MeetingUsers> MeetingUsers { get; private set; }
        public Proceedings Proceedings { get; private set; }


        public Meeting WithTitle(String title)
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Meeting WithStartDate(DateTime startTime)
        {
            HoldingTime = new DateTimeRange(startTime, HoldingTime.To);
            return this;
        }
        public Meeting WithEndDate(DateTime endDate)
        {
            HoldingTime = new DateTimeRange(HoldingTime.From, endDate);
            return this;
        }
        public Meeting WithDescription(String? description)
        {
            Description = description;
            return this;
        }

    }
}
