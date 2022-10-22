using DisciplinarySystem.SharedKernel.ValueObjects.Guards;

namespace DisciplinarySystem.SharedKernel.ValueObjects
{
    public class NationalCode
    {
        public NationalCode(string value)
        {
            Value = Guard.Against.InvalidNationalCode(value, nameof(Value));
        }

        public string Value { get; private set; }

        public static implicit operator string(NationalCode nationalCode) => nationalCode.Value;
        public static implicit operator NationalCode(string value) => new NationalCode(value);
    }
}
