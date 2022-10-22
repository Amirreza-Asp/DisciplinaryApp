using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.FinalVotes;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes.Dtos
{
    public class GetFinalVoteDto
    {
        public string Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public string Verdict { get; set; }
        public string Violation { get; set; }
        public IEnumerable<GetFinalVoteDocumentDto> Documents { get; set; }

        public static GetFinalVoteDto Create(FinalVote entity) =>
            new GetFinalVoteDto
            {
                CreateTime = entity.CreateTime,
                Description = entity.Description,
                Verdict = entity.Verdict.Title,
                Violation = entity.Violation.Title,
                Documents = GetFinalVoteDocumentDto.Create(entity.Documents)
            };
    }


    public class GetFinalVoteDocumentDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetFinalVoteDocumentDto> Create(IEnumerable<FinalVoteDocument> entities) =>
            entities.Select(entity => new GetFinalVoteDocumentDto
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

}
