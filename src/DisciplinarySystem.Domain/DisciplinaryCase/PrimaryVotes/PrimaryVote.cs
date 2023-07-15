using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Domain.PrimaryVotes
{
    public class PrimaryVote : BaseEntity<Guid>
    {
        public PrimaryVote(string description, long verdictId, Guid violationId, bool isClosed)
        {
            Id = Guid.NewGuid();
            Description = Guard.Against.NullOrEmpty(description);
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            ViolationId = Guard.Against.Default(violationId);
            CreateTime = DateTime.Now;
            IsClosed = isClosed;
        }

        public String Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public bool IsClosed { get; set; }
        public long VerdictId { get; private set; }
        public Guid ViolationId { get; set; }
        public Verdict Verdict { get; set; }
        public Violation Violation { get; set; }
        public ICollection<PrimaryVoteDocument> Documents { get; private set; }


        public PrimaryVote WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public PrimaryVote WithVerdictId(long verdictId)
        {
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            return this;
        }
        public PrimaryVote WithViolationId(Guid violationId)
        {
            ViolationId = Guard.Against.Default(violationId);
            return this;
        }

        public PrimaryVote WithIsClosed(bool isClosed)
        {
            IsClosed = isClosed;
            return this;
        }
    }
}
