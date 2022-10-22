using DisciplinarySystem.Domain.Epistles;

namespace DisciplinarySystem.Application.Epistles.ViewModels
{
    public class GetInformedDocument
    {
        public Guid Id { get; set; }
        public String Name { get; set; }
        public String File { get; set; }
        public DateTime CreateDate { get; set; }

        public static GetInformedDocument Create(EpistleDocument complaintDocument)
        {
            return new GetInformedDocument
            {
                Id = complaintDocument.Id,
                File = complaintDocument.File.Name,
                Name = complaintDocument.Name,
                CreateDate = complaintDocument.SendTime
            };
        }
    }
}
