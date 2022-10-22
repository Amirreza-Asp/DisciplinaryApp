using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.SharedKernel.Events
{
    public class RemoveFileEvent : BaseDomainEvent
    {
        public RemoveFileEvent(string path, string name)
        {
            Path = Guard.Against.NullOrEmpty(path);
            Name = Guard.Against.NullOrEmpty(name);
        }

        public Guid Id { get; private set; } = Guid.NewGuid();
        public string Path { get; private set; }
        public string Name { get; private set; }
    }
}
