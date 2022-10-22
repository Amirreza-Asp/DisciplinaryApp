using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.FinalVotes
{
    public class FinalVoteDocument : BaseEntity<Guid>
    {
        public FinalVoteDocument(Guid finalVoteId, string name, Document file)
        {
            Id = Guid.NewGuid();
            FinalVoteId = Guard.Against.Default(finalVoteId, nameof(FinalVoteId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private FinalVoteDocument() { }

        public Guid FinalVoteId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public FinalVote FinalVote { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.FinalVoteDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.FinalVoteDocumentPath, File.Name));
        }
    }
}
