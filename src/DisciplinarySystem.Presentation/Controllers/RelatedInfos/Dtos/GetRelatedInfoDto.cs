using DisciplinarySystem.Domain.PrimaryVotes;
using DisciplinarySystem.Domain.RelatedInfos;
using DisciplinarySystem.Presentation.Controllers.PrimaryVotes.Dtos;

namespace DisciplinarySystem.Presentation.Controllers.RelatedInfos.Dtos
{
    public class GetRelatedInfoDto
    {
        public DateTime CreateDate { get; set; }
        public String Subject { get; set; }
        public String Description { get; set; }
        public IEnumerable<GetRelatedInfoDocumentDto> Documents { get; set; }

        public static GetRelatedInfoDto Create(RelatedInfo entity) =>
            new GetRelatedInfoDto
            {
                CreateDate = entity.CreateDate,
                Description = entity.Description,
                Subject = entity.Subject,
                Documents = GetRelatedInfoDocumentDto.Create(entity.Documents)
            };

        public class GetRelatedInfoDocumentDto
        {
            public Guid Id { get; set; }
            public string Name { get; set; }
            public DateTime SendTime { get; set; }

            public static IEnumerable<GetRelatedInfoDocumentDto> Create(IEnumerable<RelatedInfoDocument> entities) =>
                entities.Select(entity => new GetRelatedInfoDocumentDto
                {
                    Id = entity.Id,
                    Name = entity.Name,
                    SendTime = entity.SendTime
                });
        }
    }


}
