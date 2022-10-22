using DisciplinarySystem.Domain.Objections;

namespace DisciplinarySystem.Application.Objections.ViewModels
{
    public class UpdateObjection : CreateObjection
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; set; }
        public IEnumerable<ObjectionDocument>? CurrentDocuments { get; set; }


        public static UpdateObjection Create(Objection entity) =>
            new UpdateObjection
            {
                Subject = entity.Subject,
                CaseId = entity.CaseId,
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Id = entity.Id,
                Result = entity.Result,
                CurrentDocuments = entity.Documents
            };
    }
}
