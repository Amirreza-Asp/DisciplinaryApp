using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects;

namespace DisciplinarySystem.SharedKernel.Events
{
    public class CreateFileEvent : BaseDomainEvent
    {
        public CreateFileEvent(Document document, string path)
        {
            Document = document;
            Path = Guard.Against.NullOrEmpty(path);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public Document Document { get; private set; }
        public string Path { get; private set; }
    }
}
