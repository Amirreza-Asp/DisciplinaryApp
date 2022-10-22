using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Informants.ViewModels
{
    public class CreateInformed
    {
        [Required(ErrorMessage = "نام و نام خانوادگی را وارد کنید")]
        public String FullName { get; set; }

        [Required(ErrorMessage = "شماره تلفن را وارد کنید")]
        [MaxLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        [MinLength(11, ErrorMessage = "شماره تلفن 11 رقم است")]
        public String PhoneNumber { get; set; }

        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10, ErrorMessage = "کد ملی 10 رقم است")]
        public String NationalCode { get; set; }

        [Required(ErrorMessage = "نام پدر را وارد کنید")]
        public String Father { get; set; }

        [Required(ErrorMessage = "موضوع را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "اظهارات را وارد کنید")]
        public String Statements { get; set; }

        public long CaseId { get; set; }

        public List<String>? Documents { get; set; }
    }
}
