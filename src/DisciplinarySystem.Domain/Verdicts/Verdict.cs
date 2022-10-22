using DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes;
using DisciplinarySystem.Domain.FinalVotes;
using DisciplinarySystem.Domain.PrimaryVotes;

namespace DisciplinarySystem.Domain.Verdicts
{
    public class Verdict : BaseEntity<long>
    {
        public Verdict(string title, string description)
        {
            Title = Guard.Against.NullOrEmpty(title);
            Description = description;
        }

        public Verdict()
        {
        }

        public String Title { get; private set; }
        public String Description { get; private set; }


        public ICollection<PrimaryVote> PrimaryVotes { get; private set; }
        public ICollection<FinalVote> FinalVotes { get; private set; }
        public ICollection<CentralCommitteeVote> CentralCommitteeVotes { get; private set; }

        public Verdict WithTitle(String title)
        {
            Title = Guard.Against.NullOrEmpty(title);
            return this;
        }
        public Verdict WithDescription(String description)
        {
            Description = Guard.Against.NullOrEmpty(description);
            return this;
        }
    }
}
