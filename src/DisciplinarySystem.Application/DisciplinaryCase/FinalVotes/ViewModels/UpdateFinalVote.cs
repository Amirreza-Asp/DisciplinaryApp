using DisciplinarySystem.Domain.FinalVotes;

namespace DisciplinarySystem.Application.FinalVotes.ViewModels
{
    public class UpdateFinalVote : CreateFinalVote
    {
        public Guid Id { get; set; }
        public DateTime CreateTime { get; set; }
        public IEnumerable<FinalVoteDocument>? CurrentDocuments { get; set; }

        public static UpdateFinalVote Create(FinalVote entity) =>
            new UpdateFinalVote
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
