using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Violations
{
    public class ViolationDocument : BaseEntity<Guid>
    {
        public ViolationDocument(Guid id, Guid violationId, string name, Document file)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            ViolationId = Guard.Against.Default(violationId, nameof(ViolationId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private ViolationDocument() { }

        public Guid ViolationId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }

        public Violation Violation { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.ViolationDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.ViolationDocumentPath, File.Name));
        }

    }
}
