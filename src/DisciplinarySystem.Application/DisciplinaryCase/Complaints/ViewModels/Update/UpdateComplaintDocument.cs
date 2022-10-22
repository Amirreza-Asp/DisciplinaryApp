using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Application.Complaints.ViewModels.Update
{
    public class UpdateComplaintDocument
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String File { get; set; }
        public DateTime CreateDate { get; set; }

        public static UpdateComplaintDocument CreateWithComplaintDocument(ComplaintDocument complaintDocument)
        {
            return new UpdateComplaintDocument
            {
                Id = complaintDocument.Id,
                File = complaintDocument.File.Name,
                Name = complaintDocument.Name,
                CreateDate = complaintDocument.SendTime
            };
        }
    }
}
