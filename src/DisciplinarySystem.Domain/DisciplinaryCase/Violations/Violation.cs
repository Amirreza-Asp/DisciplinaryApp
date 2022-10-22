using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.PrimaryVotes;

namespace DisciplinarySystem.Domain.Violations
{
    public class Violation : BaseEntity<Guid>
    {
        public Violation(Guid id, Guid categoryId, string title, string definition, long caseId, string? vote)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            CategoryId = Guard.Against.Default(categoryId, nameof(CategoryId));
            Title = Guard.Against.NullOrEmpty(title, nameof(Title));
            Definition = Guard.Against.NullOrEmpty(definition, nameof(Definition));
            CaseId = Guard.Against.NegativeOrZero(caseId);
            Vote = vote;
        }

        private Violation()
        {

        }

        public Guid CategoryId { get; private set; }
        public string Title { get; private set; }
        public string Definition { get; private set; }
        public DateTime CreateDate { get; private set; }
        public DateTime? UpdateDate { get; set; }
        public String? Vote { get; private set; }
        public long CaseId { get; private set; }

        public ViolationCategory Category { get; private set; }
        public Case Case { get; private set; }
        public FinalVote FinalVote { get; private set; }
        public PrimaryVote PrimaryVote { get; private set; }
        public CentralCommitteeVote CentralCommitteeVote { get; private set; }
        public ICollection<ViolationDocument> Documents { get; private set; }

        public Violation WithCategoryId(Guid categoryId)
        {
            CategoryId = Guard.Against.Default(categoryId, nameof(CategoryId));
            return this;
        }
        public Violation WithTitle(String title)
        {
            Title = Guard.Against.NullOrEmpty(title, nameof(Title));
            return this;
        }
        public Violation WithDefinition(String definition)
        {
            Definition = Guard.Against.NullOrEmpty(definition, nameof(Definition));
            return this;
        }
        public Violation WithVote(String? vote)
        {
            Vote = vote;
            return this;
        }

        public Violation WithUpdateDate(DateTime updateDate)
        {
            UpdateDate = updateDate;
            return this;
        }

    }
}
