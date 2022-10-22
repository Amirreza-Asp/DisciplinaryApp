using DisciplinarySystem.Domain.Meetings;

namespace DisciplinarySystem.Application.Meetings.ViewModels
{
	public class UpdateProccedings : CreateProceedings
	{
		public Guid Id { get; set; }
		public DateTime CreateDate { get; set; }


		public static UpdateProccedings Create(Proceedings entity) =>
			new UpdateProccedings
			{
				Id = entity.Id,
				CreateDate = entity.CreateDate,
				Description = entity.Description,
				Title = entity.Title,
				MeetingId = entity.MeetingId
			};
	}
}
