namespace DisciplinarySystem.Domain.Users
{
    public class Role : BaseEntity<Guid>
    {
        public Role(string title, string description) : this(Guid.NewGuid(), title, description) { }

        public Role(Guid id, string title, string description)
        {
            Id = Guard.Against.Default(id, nameof(Id));
            Title = Guard.Against.NullOrEmpty(title, nameof(Title));
            Description = Guard.Against.NullOrEmpty(description, nameof(Description));
        }

        private Role() { }

        public string Title { get; private set; }
        public string Description { get; private set; }

        public ICollection<User> Users { get; set; }
    }
}
