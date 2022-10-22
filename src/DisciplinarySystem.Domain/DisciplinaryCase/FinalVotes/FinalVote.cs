using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Domain.FinalVotes
{
    public class FinalVote : BaseEntity<Guid>
    {
        public FinalVote(string description, long verdictId, Guid violationId)
        {
            Id = Guid.NewGuid();
            Description = Guard.Against.NullOrEmpty(description);
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            ViolationId = Guard.Against.Default(violationId);
            CreateTime = DateTime.Now;
        }

        public String Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public long VerdictId { get; private set; }
        public Guid ViolationId { get; set; }

        public Verdict Verdict { get; set; }
        public Violation Violation { get; set; }
        public ICollection<FinalVoteDocument> Documents { get; private set; }

        public FinalVote WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public FinalVote WithVerdictId(long verdictId)
        {
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            return this;
        }
        public FinalVote WithViolationId(Guid violationId)
        {
            ViolationId = Guard.Against.Default(violationId);
            return this;
        }

    }
}
