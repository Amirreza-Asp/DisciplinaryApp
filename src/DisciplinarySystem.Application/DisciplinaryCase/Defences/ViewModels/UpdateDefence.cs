using DisciplinarySystem.Application.Complaints.ViewModels.Create;
using DisciplinarySystem.Domain.Defences;

namespace DisciplinarySystem.Application.Defences.ViewModels
{
    public class UpdateDefence : CreateDefence
    {

        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public List<DefenceDocument>? CurrentDocuments { get; set; }


        public static UpdateDefence Create(Defence entity) =>
           new UpdateDefence
           {
               Id = entity.Id,
               Subject = entity.Subject,
               Description = entity.Description,
               CaseId = entity.CaseId,
               CreateDate = entity.CreateDate,
               CurrentDocuments = entity.Documents.ToList(),
               Complaining = CreateComplaining.Create(entity.Case.Complaint.Complaining)
           };
    }
}
