using DisciplinarySystem.Application.FinalVotes.ViewModels;
using DisciplinarySystem.Domain.FinalVotes;

namespace DisciplinarySystem.Presentation.Controllers.FinalVotes.ViewModels
{
    public class FinalVoteDetails : CreateFinalVote
    {
        public DateTime CreateTime { get; set; }
        public IEnumerable<FinalVoteDocument>? CurrentDocuments { get; set; }
        public String VerdictTitle { get; set; }
        public String ViolationTitle { get; set; }
        public long CaseId { get; set; }

        public static FinalVoteDetails Create(FinalVote entity) =>
            new FinalVoteDetails
            {
                CreateTime = entity.CreateTime,
                ViolationTitle = entity.Violation.Title,
                CaseId = entity.Violation.CaseId,
                Description = entity.Description,
                VerdictTitle = entity.Verdict.Title,
                CurrentDocuments = entity.Documents
            };
    }
}
