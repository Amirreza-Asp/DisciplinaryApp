namespace DisciplinarySystem.Domain.Users.Exceptions
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
