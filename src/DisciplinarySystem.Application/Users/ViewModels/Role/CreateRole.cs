using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Users.ViewModels.Role
{
    public class CreateRole
    {
        [Required(ErrorMessage = "لطفا عنوان نقش را وارد کنید")]
        [Display(Name = "عنوان")]
        public String Title { get; set; }


        [Required(ErrorMessage = "لطفا توضیحات نقش را وارد کنید")]
        [Display(Name = "توضیحات")]
        public String Description { get; set; }
    }
}
