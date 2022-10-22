using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.RelatedInfos.ViewModels
{
    public class CreateRelatedInfo
    {
        [Required(ErrorMessage = "موضوع را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "شرح را وارد کنید")]
        public String Description { get; set; }

        public List<String>? Documents { get; set; }

        public long CaseId { get; set; }

    }
}
