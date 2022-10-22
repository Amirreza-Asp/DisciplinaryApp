namespace DisciplinarySystem.Domain.Meetings
{
	public class Proceedings : BaseEntity<Guid>
	{
		public Proceedings(string title, string description, Guid meetingId)
		{
			Id = Guid.NewGuid();
			CreateDate = DateTime.Now;
			Title = Guard.Against.NullOrEmpty(title);
			Description = Guard.Against.NullOrEmpty(description);
			MeetingId = Guard.Against.Default(meetingId);
		}

		public String Title { get; private set; }
		public String Description { get; private set; }
		public DateTime CreateDate { get; private set; }

		public Guid MeetingId { get; set; }
		public Meeting Meeting { get; set; }

		public Proceedings WithTitle(String title)
		{
			Title = Guard.Against.NullOrEmpty(title);
			return this;
		}
		public Proceedings WithDescription(String description)
		{
			Description = Guard.Against.NullOrEmpty(description);
			return this;
		}


	}
}
