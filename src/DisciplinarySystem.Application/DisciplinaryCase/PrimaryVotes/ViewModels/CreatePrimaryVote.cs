using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.PrimaryVotes.ViewModels
{
    public class CreatePrimaryVote
    {

        [Required(ErrorMessage = "نوع حکم را مشخص کنید")]
        public long VerdictId { get; set; }

        [Required(ErrorMessage = "شرح را وارد کنید")]
        public String Description { get; set; }

        public List<String>? Documents { get; set; }

        [Required(ErrorMessage = "تخلف را وارد کنید")]
        public Guid ViolationId { get; set; }
        public long CaseId { get; set; }


        public CreateComplaining Complaining { get; set; }
        public IEnumerable<String>? Users { get; set; }

        public IEnumerable<SelectListItem>? Violations { get; set; }
        public IEnumerable<SelectListItem>? Verdicts { get; set; }
    }
}
