using DisciplinarySystem.Domain.Complaints;
using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;

namespace DisciplinarySystem.Presentation.Controllers.CentralCommitteeVotes.ViewModels
{
    public class GetCommitteeVoteApi
    {
        public String Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public String Verdict { get; set; }
        public String Violation { get; set; }
        public IEnumerable<GetCommitteeVoteDocumentApi> Documents { get; set; }

        public static GetCommitteeVoteApi Create(CentralCommitteeVote entity) =>
            new GetCommitteeVoteApi
            {
                CreateTime = entity.CreateTime,
                Description = entity.Description,
                Verdict = entity.Verdict.Title,
                Violation = entity.Violation.Title,
                Documents = GetCommitteeVoteDocumentApi.Create(entity.Documents)
            };
    }


    public class GetCommitteeVoteDocumentApi
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public DateTime SendTime { get; set; }

        public static IEnumerable<GetCommitteeVoteDocumentApi> Create(IEnumerable<CentralCommitteeVoteDocument> entities) =>
            entities.Select(entity => new GetCommitteeVoteDocumentApi
            {
                Id = entity.Id,
                Name = entity.Name,
                SendTime = entity.SendTime
            });
    }

}
