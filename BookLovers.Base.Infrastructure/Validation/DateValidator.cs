using System;

namespace BookLovers.Base.Infrastructure.Validation
{
    public static class DateValidator
    {
        public static bool IsValidDate(DateTime date)
        {
            return (date.Year <= 2100) && date.Month > 0 &&
                   date.Month <= 12 && (date.Day >= 1 || date.Day <= 31);
        }
    }
}