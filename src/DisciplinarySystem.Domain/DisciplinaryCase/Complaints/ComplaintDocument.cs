using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Complaints
{
    public class ComplaintDocument : BaseEntity<Guid>
    {
        public ComplaintDocument(Guid id, long complaintId, string name, Document file)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            ComplaintId = Guard.Against.Default(complaintId, nameof(complaintId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private ComplaintDocument() { }

        public long ComplaintId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }

        public Complaint Complaint { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.ComplaintDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.ComplaintDocumentPath, File.Name));
        }
    }
}
