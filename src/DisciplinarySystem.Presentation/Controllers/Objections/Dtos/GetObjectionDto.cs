using DisciplinarySystem.Domain.Invitations;
using DisciplinarySystem.Domain.Objections;
using DisciplinarySystem.Domain.Users;
using DisciplinarySystem.Presentation.Controllers.Invitations.Dtos;

namespace DisciplinarySystem.Presentation.Controllers.Objections.Dtos
{
    public class GetObjectionDto
    {
        public Guid Id { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public DateTime CreateDate { get; set; }
        public String Result { get; set; }
        public IEnumerable<GetObjectionDocumentDto> Documents { get; set; }

        public static GetObjectionDto Create(Objection entity) =>
            new GetObjectionDto
            {
                Id = entity.Id,
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Subject = entity.Subject,
                Result = entity.Result,
                Documents = GetObjectionDocumentDto.Create(entity.Documents)
            };
    }

    public class GetObjectionDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetObjectionDocumentDto> Create(IEnumerable<ObjectionDocument> entities) =>
            entities.Select(entity => new GetObjectionDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }
}
