using DisciplinarySystem.SharedKernel;
using DisciplinarySystem.SharedKernel.ValueObjects;

namespace DisciplinarySystem.Domain.Defences
{
    public class DefenceDocument : BaseEntity<Guid>
    {
        public DefenceDocument(Guid defenceId, string name, Document file)
        {
            Id = Guid.NewGuid();
            DefenceId = Guard.Against.Default(defenceId, nameof(DefenceId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private DefenceDocument() { }

        public Guid DefenceId { get; }
        public string Name { get; }
        public Document File { get; private set; }
        public DateTime SendTime { get; }
        public Defence Defence { get; private set; }

        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.DefenceDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.DefenceDocumentPath, File.Name));
        }

        public void GetFile(String upload)
        {
            var path = upload + SD.DefenceDocumentPath + File.Name;
            var bytes = System.IO.File.ReadAllBytes(path);
            File = new Document(File.Name, bytes);
        }
    }
}
