using DisciplinarySystem.Domain.Complaints.Enums;

namespace DisciplinarySystem.Application.Complaints.Helpers
{
	public static class ResultConvertor
	{

		public static String ToPersian(this ComplaintResult result) => result switch
		{
			ComplaintResult.Archive => "بایگانی",
			ComplaintResult.Filing => "تشکیل پرونده",
			ComplaintResult.SeeCase => "مشاهده پرونده",
			_ => "نا مشخص"
		};

	}
}
