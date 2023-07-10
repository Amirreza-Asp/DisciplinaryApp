using DisciplinarySystem.Application.Complaints.Helpers;
using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Complaints.Enums;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Update
{
    public class UpdateComplaint
    {

        public long Id { get; set; }
        public DateTime CreateDate { get; set; }

        [Display(Name = "موضوع")]
        [Required(ErrorMessage = "لطفا عنوان شکایت را وارد کنید")]
        public string Subject { get; set; }

        [Display(Name = "شرح")]
        [Required(ErrorMessage = "لطفا شرح شکایت را وارد کنید")]
        public string Description { get; set; }

        public List<SelectListItem>? Results { get; set; }
        public int Result { get; set; }


        public UpdateComplaining Complaining { get; set; }
        public UpdatePlaintiff Plaintiff { get; set; }
        public List<UpdateComplaintDocument>? CurrentDocuments { get; set; }
        public List<string>? NewDocuments { get; set; }
        public long CaseId { get; set; }


        public static UpdateComplaint CreateWith(Complaint complaint)
        {
            return new UpdateComplaint
            {
                Id = complaint.Id,
                Description = complaint.Description,

                Results = new List<SelectListItem>()
                {
                    new SelectListItem(){Text = ComplaintResult.Archive.ToPersian() , Value = "1"},
                    new SelectListItem(){Text = ComplaintResult.Filing.ToPersian() , Value ="2"}
                },
                Result = (int)complaint.Result,
                CurrentDocuments = complaint.Documents.
                    Select(u => UpdateComplaintDocument.CreateWithComplaintDocument(u)).ToList(),
                CreateDate = complaint.CreateDate,
                Subject = complaint.Title,
                Complaining = UpdateComplaining.CreateWitComplaining(complaint.Complaining),
                Plaintiff = UpdatePlaintiff.CreateWith(complaint.Plaintiff)
            };
        }

    }
}
