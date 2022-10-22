using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects.Guards;

namespace DisciplinarySystem.SharedKernel.ValueObjects
{
    public class Document : ValueObject<Document>
    {
        public Document(string name, byte[] file)
        {

            name = Guard.Against.InvalidFileName(name);
            Name = $"{Guid.NewGuid()}{Path.GetExtension(name)}";
            File = Guard.Against.NullOrEmpty(file).ToArray();
        }

        private Document() { }

        public string Name { get; }
        public byte[] File { get; }

        protected override bool EqualsCore(Document other)
        {
            return Name == other.Name && File == other.File;
        }

        public static implicit operator string(Document document) => document.Name;
    }
}
