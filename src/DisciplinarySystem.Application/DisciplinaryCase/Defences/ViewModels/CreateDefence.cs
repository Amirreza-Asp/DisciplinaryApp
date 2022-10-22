using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Domain.Defences;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Defences.ViewModels
{
    public class CreateDefence
    {
        [Required(ErrorMessage = "موضوع دفاعیات را وارد کنید")]
        public String Subject { get; set; }

        [Required(ErrorMessage = "شرح دفاعیات را وارد کنید")]
        public String Description { get; set; }

        public List<String>? Documents { get; set; }

        public CreateComplaining Complaining { get; set; }

        public long CaseId { get; set; }



        public static CreateDefence Create(Defence entity) =>
            new CreateDefence
            {
                Subject = entity.Subject,
                Description = entity.Description,
                Complaining = CreateComplaining.Create(entity.Case.Complaint.Complaining)
            };
    }
}
