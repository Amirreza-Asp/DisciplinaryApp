using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.RelatedInfos
{
    public class RelatedInfoDocument : BaseEntity<Guid>
    {
        public RelatedInfoDocument(Guid relatedInfoId, string name, Document file)
        {
            Id = Guid.NewGuid();
            RelatedInfoId = Guard.Against.Default(relatedInfoId, nameof(RelatedInfoId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private RelatedInfoDocument() { }

        public Guid RelatedInfoId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }
        public RelatedInfo RelatedInfo { get; private set; }

        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.RelatedInfoDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.RelatedInfoDocumentPath, File.Name));
        }
    }
}
