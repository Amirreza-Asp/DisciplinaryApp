using DisciplinarySystem.Domain.Meetings;

namespace DisciplinarySystem.Application.Meetings.ViewModels
{
	public class UpdateMeeting : CreateMeeting
	{
		public Guid Id { get; set; }
		public DateTime CreateTime { get; set; }
		public List<(String Name, Guid Id)>? SelectedUsers { get; set; }


		public static UpdateMeeting Create(Meeting entity) =>
			new UpdateMeeting
			{
				Id = entity.Id,
				End = entity.HoldingTime.To.TimeOfDay.ToString().Substring(0, 5),
				Start = entity.HoldingTime.From.TimeOfDay.ToString().Substring(0, 5),
				CreateTime = entity.CreateDate,
				Description = entity.Description,
				Title = entity.Title,
				MeetingDate = entity.HoldingTime.From,
			};
	}
}
