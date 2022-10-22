using DisciplinarySystem.SharedKernel;

namespace DisciplinarySystem.Domain.Invitations
{
    public class InvitationDocument : BaseEntity<Guid>
    {
        public InvitationDocument(Guid id, Guid invitationId, string name, Document file)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            InvitationId = Guard.Against.Default(invitationId, nameof(InvitationId));
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            File = file;
            SendTime = DateTime.Now;
        }

        private InvitationDocument() { }

        public Guid InvitationId { get; }
        public string Name { get; }
        public Document File { get; }
        public DateTime SendTime { get; }

        public Invitation Invitation { get; private set; }


        public void CreateFile()
        {
            Events.Add(new CreateFileEvent(File, SD.InvitationDocumentPath));
        }

        public void RemoveFile()
        {
            Events.Add(new RemoveFileEvent(SD.InvitationDocumentPath, File.Name));
        }

    }
}
