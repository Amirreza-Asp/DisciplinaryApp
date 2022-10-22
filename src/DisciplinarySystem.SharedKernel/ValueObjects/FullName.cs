using DisciplinarySystem.SharedKernel.Common;

namespace DisciplinarySystem.SharedKernel.ValueObjects
{
    public class FullName : ValueObject<FullName>
    {
        public FullName(string name, string family)
        {
            Name = Guard.Against.NullOrEmpty(name, nameof(Name));
            Family = Guard.Against.NullOrEmpty(family, nameof(Family));
        }

        public string Name { get; private set; }
        public string Family { get; private set; }

        protected override bool EqualsCore(FullName other)
        {
            return Name == other.Name && Family == other.Family;
        }

        public static implicit operator string(FullName fullName) => $"{fullName.Name} {fullName.Family}";
    }
}
