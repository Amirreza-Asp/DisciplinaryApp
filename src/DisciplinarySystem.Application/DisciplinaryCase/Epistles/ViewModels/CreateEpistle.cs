using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Epistles.ViewModels
{
    public class CreateEpistle
    {
        [Required(ErrorMessage = "نوع نامه را وارد کنید")]
        public string Type { get; set; }

        [Required(ErrorMessage = "موضوع را وارد کنید")]
        public string Subject { get; set; }

        [Required(ErrorMessage = "نام فرستنده را وارد کنید")]
        public String Sender { get; set; }

        [Required(ErrorMessage = "نام گیرنده را وارد کنید")]
        public String Reciver { get; set; }

        [Required(ErrorMessage = "شرح نامه را وارد کنید")]
        public String Description { get; set; }

        public long? CaseId { get; set; }

        public long? ComplaintId { get; set; }

        public List<String>? Documents { get; set; }

        public IEnumerable<SelectListItem>? Positions { get; set; }
    }
}
