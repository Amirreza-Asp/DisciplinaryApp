using DisciplinarySystem.Domain.Complaints;

namespace DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels
{
    public class ComplaintDetailsVM
    {
        public Complaint Complaint { get; set; }
        public long CaseId { get; set; }
    }
}
