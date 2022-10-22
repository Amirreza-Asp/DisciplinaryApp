namespace DisciplinarySystem.Domain.Violations
{
    public class ViolationCategory : BaseEntity<Guid>
    {
        public ViolationCategory(string title, string description) : this(Guid.NewGuid(), title, description) { }

        public ViolationCategory(Guid id, string title, string description)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            Title = Guard.Against.NullOrEmpty(title, nameof(Title));
            Description = Guard.Against.NullOrEmpty(description, nameof(Title));
        }

        public string Title { get; }
        public string Description { get; }

        public ICollection<Violation> Violations { get; private set; }
    }
}
