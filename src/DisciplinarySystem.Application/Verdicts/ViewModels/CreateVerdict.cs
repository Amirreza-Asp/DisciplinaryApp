using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Verdicts.ViewModels
{
    public class CreateVerdict
    {
        [Required(ErrorMessage = "عنوان حکم را وارد کنید")]
        [Display(Name = "عنوان")]
        public String Title { get; set; }


        [Required(ErrorMessage = "توضیحات حکم را وارد کنید")]
        [Display(Name = "توضیحات")]
        public String Description { get; set; }
    }
}
