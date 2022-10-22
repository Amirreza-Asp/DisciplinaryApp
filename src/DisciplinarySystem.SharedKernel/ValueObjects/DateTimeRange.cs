using DisciplinarySystem.SharedKernel.Common;
using DisciplinarySystem.SharedKernel.ValueObjects.Exceptions;

namespace DisciplinarySystem.SharedKernel.ValueObjects
{
    public class DateTimeRange : ValueObject<DateTimeRange>
    {
        public DateTimeRange(DateTime from, DateTime to)
        {
            if (from > to)
                throw new InvalidDateTimeRangeException("Start date not be greater than end date");

            From = from;
            To = to;
        }

        public DateTime From { get; private set; }
        public DateTime To { get; private set; }

        protected override bool EqualsCore(DateTimeRange other)
        {
            return From == other.From || To == other.To;

        }

    }
}
