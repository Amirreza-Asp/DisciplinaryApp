using DisciplinarySystem.Domain.Complaints;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Create
{
    public class CreateComplaining
    {
        [Display(Name = "نام و نام خانوادگی")]
        [Required(ErrorMessage = "لطفا نام را وارد کنید")]
        public string FullName { get; set; }

        [Display(Name = "شماره دانشجویی")]
        [Required(ErrorMessage = "شماره دانشجویی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "شماره دانشجویی 9  یا 10 رقم است")]
        [MinLength(9 , ErrorMessage = "شماره دانشجویی 9  یا 10 رقم است")]
        public string StudentNumber { get; set; }

        [Display(Name = "کد ملی")]
        [Required(ErrorMessage = "کد ملی را وارد کنید")]
        [MaxLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        [MinLength(10 , ErrorMessage = "کد ملی 10 رقم است")]
        public string NationalCode { get; set; }

        public string? Grade { get; set; }
        public string? EducationGroup { get; set; }
        public string? College { get; set; }
        public String? Father { get; set; }


        public static CreateComplaining Create ( Complaining entity ) =>
            new CreateComplaining()
            {
                FullName = entity.FullName ,
                StudentNumber = entity.StudentNumber ,
                NationalCode = entity.NationalCode ,
                Grade = entity.Grade ,
                EducationGroup = entity.EducationalGroup ,
                College = entity.College ,
            };
    }
}
