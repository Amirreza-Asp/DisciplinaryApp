using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Create
{
    public class CreatePlaintiff
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "شماره تلفن")]
        [Required(ErrorMessage = "شماره تلفن را وارد کنید")]
        [MaxLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        [MinLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        public string PhoneNumber { get; set; }

        [Display(Name = "آدرس")]
        [Required(ErrorMessage = "آدرس را وارد کنید")]
        public string Address { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        public string NationalCode { get; set; }
    }
}
