using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Informants
{
    public class InformedDocument : BaseEntity<Guid>
    {
        public InformedDocument(Guid id, Guid informedId, string name, Document file)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            InformedId = Guard.Against.Default(informedId, nameof(InformedId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private InformedDocument() { }

        public Guid InformedId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public Informed Informed { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.InformedDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.InformedDocumentPath, File.Name));
        }
    }
}
