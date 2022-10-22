using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Violations.ViewModels.ViolationCategory
{
    public class CreateViolationCategory
    {
        [Required(ErrorMessage = "لطفا عنوان را وارد کنید")]
        [Display(Name = "عنوان")]
        public string Title { get; set; }


        [Required(ErrorMessage = "لطفا توضیحات را وارد کنید")]
        [Display(Name = "توضیحات")]
        public string Description { get; set; }
    }
}
