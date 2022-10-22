using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Objections.ViewModels
{
    public class CreateObjection
    {
        [Required(ErrorMessage = "موضوع اعتراض را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "نتیجه اعتراض را وارد کنید")]
        public String Result { get; set; }

        [Required(ErrorMessage = "شرح اعتراض را وارد کنید")]
        public String Description { get; set; }

        public long CaseId { get; set; }

        public List<String>? Documents { get; set; }
    }
}
