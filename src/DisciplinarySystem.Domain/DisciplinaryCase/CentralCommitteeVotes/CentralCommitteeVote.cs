using DisciplinarySystem.Domain.Verdicts;
using DisciplinarySystem.Domain.Violations;

namespace DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes
{
    public class CentralCommitteeVote : BaseEntity<Guid>
    {
        public CentralCommitteeVote(string description, long verdictId, Guid violationId)
        {
            Id = Guid.NewGuid();
            Description = Guard.Against.NullOrEmpty(description);
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            ViolationId = Guard.Against.Default(violationId);
            CreateTime = DateTime.Now;
        }
        private CentralCommitteeVote() { }

        public String Description { get; private set; }
        public DateTime CreateTime { get; private set; }
        public long VerdictId { get; private set; }
        public Guid ViolationId { get; set; }

        public Verdict Verdict { get; set; }
        public Violation Violation { get; set; }
        public ICollection<CentralCommitteeVoteDocument> Documents { get; private set; }

        public CentralCommitteeVote WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
        public CentralCommitteeVote WithVerdictId(long verdictId)
        {
            VerdictId = Guard.Against.NegativeOrZero(verdictId);
            return this;
        }
        public CentralCommitteeVote WithViolationId(Guid violationId)
        {
            ViolationId = Guard.Against.Default(violationId);
            return this;
        }

    }
}
