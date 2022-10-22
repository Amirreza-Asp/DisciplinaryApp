using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Authentication.Dtos
{
    public class ForgetPasswordDto
    {
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        public String NationalCode { get; set; }

        public int SecretCode { get; set; }
    }
}
