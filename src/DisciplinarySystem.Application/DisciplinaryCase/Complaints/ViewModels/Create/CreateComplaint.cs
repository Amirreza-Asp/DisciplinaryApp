using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Create
{
    public class CreateComplaint
    {
        [Display(Name = "موضوع")]
        [Required(ErrorMessage = "لطفا عنوان شکایت را وارد کنید")]
        public string Subject { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا شرح شکایت را وارد کنید")]
        public string Description { get; set; }

        public CreateComplaining Complaining { get; set; }
        public CreatePlaintiff Plaintiff { get; set; }
        public List<string>? Documents { get; set; }

        public long CaseId { get; set; }
    }
}
