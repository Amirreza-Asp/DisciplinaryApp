using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Authentication.Dtos
{
    public class LoginDto
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public String UserName { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        public String Password { get; set; }
    }
}
