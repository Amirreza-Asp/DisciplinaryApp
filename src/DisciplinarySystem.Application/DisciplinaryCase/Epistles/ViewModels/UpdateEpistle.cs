using DisciplinarySystem.Domain.Epistles;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Epistles.ViewModels
{
    public class UpdateEpistle
    {
        public long Id { get; set; }
        public DateTime CreateDate { get; set; }

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

        public List<GetInformedDocument>? CurrentDocuments { get; set; }

        public List<String>? NewDocuments { get; set; }

        public bool WithSteps { get; set; }

        public IEnumerable<SelectListItem>? Positions { get; set; }

        public static UpdateEpistle Create ( Epistle entity )
        {
            return new UpdateEpistle
            {
                Id = entity.Id ,
                Subject = entity.Subject ,
                Sender = entity.Sender ,
                Reciver = entity.Reciver ,
                Type = entity.Type ,
                CaseId = entity.CaseId ,
                ComplaintId = entity.ComplaintId ,
                Description = entity.Description ,
                CreateDate = entity.CreateDate ,
                CurrentDocuments = entity.Documents.Select(doc => GetInformedDocument.Create(doc)).ToList()
            };
        }
    }
}
