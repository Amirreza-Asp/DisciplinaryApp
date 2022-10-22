using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.PrimaryVotes;

namespace DisciplinarySystem.Presentation.Controllers.PrimaryVotes.Dtos
{
    public class GetPrimaryVoteDto
    {
        public string Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public string Verdict { get; set; }
        public string Violation { get; set; }
        public IEnumerable<GetPrimaryVoteDocumentDto> Documents { get; set; }

        public static GetPrimaryVoteDto Create(PrimaryVote entity) =>
            new GetPrimaryVoteDto
            {
                CreateTime = entity.CreateTime,
                Description = entity.Description,
                Verdict = entity.Verdict.Title,
                Violation = entity.Violation.Title,
                Documents = GetPrimaryVoteDocumentDto.Create(entity.Documents)
            };
    }


    public class GetPrimaryVoteDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetPrimaryVoteDocumentDto> Create(IEnumerable<PrimaryVoteDocument> entities) =>
            entities.Select(entity => new GetPrimaryVoteDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

}
