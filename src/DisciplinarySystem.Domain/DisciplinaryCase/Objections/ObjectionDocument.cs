using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Objections
{
    public class ObjectionDocument : BaseEntity<Guid>
    {
        public ObjectionDocument(Guid objectionId, string name, Document file)
        {
            Id = Guid.NewGuid();
            ObjectionId = Guard.Against.Default(objectionId, nameof(ObjectionId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private ObjectionDocument() { }

        public Guid ObjectionId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public Objection Objection { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.ObjectionDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.ObjectionDocumentPath, File.Name));
        }
    }
}
