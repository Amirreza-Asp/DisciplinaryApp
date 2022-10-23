using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Presentation.Controllers.Commonications.Models
{
    public class SMSDto
    {
        [Required(ErrorMessage = "شماره موبایل را وارد کنید")]
        [DataType(DataType.PhoneNumber)]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "متن پیام را وارد کنید")]
        [MaxLength(50 , ErrorMessage = "متن پیام کمتر از 50 کاراکتر است")]
        public String Content { get; set; }
    }
}
