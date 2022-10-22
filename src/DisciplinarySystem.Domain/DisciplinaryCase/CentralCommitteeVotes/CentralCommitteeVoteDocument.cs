using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.DisciplinaryCase.CentralCommitteeVotes
{
    public class CentralCommitteeVoteDocument : BaseEntity<Guid>
    {
        public CentralCommitteeVoteDocument(Guid centralCommitteeVoteId, string name, Document file)
        {
            Id = Guid.NewGuid();
            CentralCommitteeVoteId = Guard.Against.Default(centralCommitteeVoteId, nameof(CentralCommitteeVoteId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private CentralCommitteeVoteDocument() { }

        public Guid CentralCommitteeVoteId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public CentralCommitteeVote CentralCommitteeVote { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.CentralCommitteeVoteDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.CentralCommitteeVoteDocumentPath, File.Name));
        }
    }
}
