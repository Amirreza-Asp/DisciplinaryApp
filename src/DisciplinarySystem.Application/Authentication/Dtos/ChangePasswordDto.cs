using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Authentication.Dtos
{
	public class ChangePasswordDto
	{
		[Required(ErrorMessage = "رمز عبور را وارد کنید")]
		[DataType(DataType.Password)]
		public String Password { get; set; }


		[Required(ErrorMessage = "تکرار رمز عبور را وارد کنید")]
		[Compare(nameof(Password) , ErrorMessage = "تکرار رمز عبور با رمز عبور شباهت ندارد")]
		public String ConfirmPassowrd { get; set; }
	}
}
