using DisciplinarySystem.Domain.PrimaryVotes;

namespace DisciplinarySystem.Application.PrimaryVotes.ViewModels
{
    public class UpdatePrimaryVote : CreatePrimaryVote
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<PrimaryVoteDocument>? CurrentDocuments { get; set; }

        public static UpdatePrimaryVote Create(PrimaryVote entity) =>
            new UpdatePrimaryVote
            {
                Id = entity.Id,
                CreateTime = entity.CreateTime,
                CaseId = entity.Violation.CaseId,
                Description = entity.Description,
                VerdictId = entity.VerdictId,
                ViolationId = entity.ViolationId,
                CurrentDocuments = entity.Documents,
                IsClosed = entity.IsClosed
            };
    }
}
