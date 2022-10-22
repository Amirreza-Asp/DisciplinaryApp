using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Presentation.Controllers.Cases.ViewModels
{
	public class CaseInquiry
	{
		[Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
		public String FullName { get; set; }

		[Required(ErrorMessage = "کد ملی را وارد کنید")]
		[MaxLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
		[MinLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
		public String NationalCode { get; set; }


		[Required(ErrorMessage = "شماره دانشجویی را وارد کنید")]
		[MaxLength(10 , ErrorMessage = "شماره دانشجویی 9  یا 10 عدد است")]
		[MinLength(9 , ErrorMessage = "شماره دانشجویی 9  یا 10 عدد است")]
		public String StudentNumber { get; set; }
	}
}
