using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.Defences;
using DisciplinarySystem.Presentation.Controllers.Complaints.ViewModels;

namespace DisciplinarySystem.Presentation.Controllers.Defences.ViewModels
{
    public class GetDefenceApi
    {
        public Guid Id { get; set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; private set; }
        public String Subject { get; private set; }
        public String Description { get; private set; }
        public IEnumerable<GetDefenceDocumentApi> Documents { get; set; }

        public static GetDefenceApi Create(Defence entity) =>
            new GetDefenceApi
            {
                Subject = entity.Subject,
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Id = entity.Id,
                UpdateDate = entity.UpdateDate,
                Documents = GetDefenceDocumentApi.Create(entity.Documents)
            };
    }

    public class GetDefenceDocumentApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetDefenceDocumentApi> Create(IEnumerable<DefenceDocument> entities) =>
            entities.Select(entity => new GetDefenceDocumentApi
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

}
