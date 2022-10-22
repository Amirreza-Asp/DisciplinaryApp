using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Users.ViewModels.Role
{
    public class UpdateRole
    {
        public Guid Id { get; set; }

        [Required(ErrorMessage = "لطفا عنوان نقش را وارد کنید")]
        [Display(Name = "عنوان")]
        public String Title { get; set; }


        [Required(ErrorMessage = "لطفا توضیحات نقش را وارد کنید")]
        [Display(Name = "توضیحات")]
        public String Description { get; set; }
    }
}
