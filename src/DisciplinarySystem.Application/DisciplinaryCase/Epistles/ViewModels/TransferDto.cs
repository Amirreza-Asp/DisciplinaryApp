using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.DisciplinaryCase.Epistles.ViewModels
{
    public class TransferDto
    {
        [Required(ErrorMessage = "نام را وارد کنید")]
        public String Name { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقمی است")]
        public String NationalCode { get; set; }
    }
}
