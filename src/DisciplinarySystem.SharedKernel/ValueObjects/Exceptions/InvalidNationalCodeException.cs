namespace DisciplinarySystem.SharedKernel.ValueObjects.Exceptions
{
    public class InvalidNationalCodeException : Exception
    {
        public InvalidNationalCodeException()
        {
        }

        public InvalidNationalCodeException(string? message) : base(message)
        {
        }
    }
}
