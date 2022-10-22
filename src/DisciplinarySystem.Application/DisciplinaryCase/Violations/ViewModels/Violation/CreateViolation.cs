using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Violations.ViewModels.Violation
{
    public class CreateViolation
    {
        public List<SelectListItem>? Categories { get; set; }

        [Display(Name = "نوع تخلف")]
        [Required(ErrorMessage = "نوع تخلف را وارد کنید")]
        public Guid CategoryId { get; set; }

        [Display(Name = "عنوان تخلف")]
        [Required(ErrorMessage = "عنوان تخلف را وارد کنید")]
        public string Title { get; set; }

        [Display(Name = "تعریف تخلف")]
        [Required(ErrorMessage = "شرح تخلف را وارد کنید")]
        public string Definition { get; set; }


        public List<String>? Documents { get; set; }
        public long CaseId { get; set; }
    }
}
