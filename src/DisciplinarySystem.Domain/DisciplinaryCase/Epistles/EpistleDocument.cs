using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Epistles
{
    public class EpistleDocument : BaseEntity<Guid>
    {
        public EpistleDocument(Guid id, long epistleId, string name, Document file)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            EpistleId = Guard.Against.NegativeOrZero(epistleId, nameof(epistleId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private EpistleDocument() { }

        public long EpistleId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public Epistle Epistle { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.EpistleDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.EpistleDocumentPath, File.Name));
        }
    }
}
