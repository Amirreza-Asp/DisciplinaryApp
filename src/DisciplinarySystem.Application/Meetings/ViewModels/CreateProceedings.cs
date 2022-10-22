using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Meetings.ViewModels
{
	public class CreateProceedings
	{
		[Required(ErrorMessage = "عنوان صورت جلسه را وارد کنید")]
		public String Title { get; set; }

		[Required(ErrorMessage = "تصمیمات صورت جلسه را وارد کنید")]
		public String Description { get; set; }

		public Guid MeetingId { get; set; }


		public String RequestPath { get; set; }
	}
}
