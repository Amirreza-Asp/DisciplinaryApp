namespace DisciplinarySystem.SharedKernel.ValueObjects.Exceptions
{
    public class InvalidDateTimeRangeException : Exception
    {
        public InvalidDateTimeRangeException()
        {
        }

        public InvalidDateTimeRangeException(string? message) : base(message)
        {
        }
    }
}
