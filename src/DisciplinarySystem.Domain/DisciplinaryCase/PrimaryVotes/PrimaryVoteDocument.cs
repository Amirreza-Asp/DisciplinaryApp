using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.PrimaryVotes
{
    public class PrimaryVoteDocument : BaseEntity<Guid>
    {
        public PrimaryVoteDocument(Guid primaryVoteId, string name, Document file)
        {
            Id = Guid.NewGuid();
            PrimaryVoteId = Guard.Against.Default(primaryVoteId, nameof(PrimaryVoteId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private PrimaryVoteDocument() { }

        public Guid PrimaryVoteId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public PrimaryVote PrimaryVote { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.PrimaryVoteDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.PrimaryVoteDocumentPath, File.Name));
        }
    }
}
