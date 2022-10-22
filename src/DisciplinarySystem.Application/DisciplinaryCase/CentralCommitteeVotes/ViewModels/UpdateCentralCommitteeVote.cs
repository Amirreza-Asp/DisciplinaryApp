using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;

namespace DisciplinarySystem.Application.DisciplinaryCase.CentralCommitteeVotes.ViewModels
{
    public class UpdateCentralCommitteeVote : CreateCentralCommitteeVote
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<CentralCommitteeVoteDocument>? CurrentDocuments { get; set; }

        public static UpdateCentralCommitteeVote Create(CentralCommitteeVote entity) =>
            new UpdateCentralCommitteeVote
            {
                Id = entity.Id,
                CaseId = entity.Violation.CaseId,
                CreateTime = entity.CreateTime,
                ViolationId = entity.ViolationId,
                Description = entity.Description,
                VerdictId = entity.VerdictId,
                CurrentDocuments = entity.Documents
            };
    }
}
