using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Violations.ViewModels.Violation
{
    public class UpdateViolation
    {
        public Guid Id { get; set; }
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


        public long CaseId { get; set; }
        public DateTime CreateDate { get; set; }

        public List<Domain.Violations.ViolationDocument>? CurrentDocuments { get; set; }
        public List<String>? NewDocuments { get; set; }
    }
}
