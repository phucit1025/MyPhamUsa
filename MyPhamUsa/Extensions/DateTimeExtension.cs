using System;

namespace MyPhamUsa.Extensions
{
    public static class DateTimeExtension
    {
        public static DateTime GetDateOfDayOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = (7 + (dt.DayOfWeek - startOfWeek)) % 7;
            if (startOfWeek.Equals(DayOfWeek.Sunday))
            {
                return dt.Latest().AddDays(-1 * diff).Date;
            }
            return dt.Midnight().AddDays(-1 * diff).Date;
        }

        public static DateTime Midnight(this DateTime date)
        {
            var hr = date.Hour;
            var min = date.Minute;
            var sec = date.Second;
            var milliSec = date.Millisecond;

            var midnight = date
                .AddHours(-hr)
                .AddMinutes(-min)
                .AddSeconds(-sec)
                .AddMilliseconds(-milliSec);

            return midnight;
        }

        public static DateTime Latest(this DateTime date)
        {
            var hr = date.Hour;
            var min = date.Minute;
            var sec = date.Second;
            var milliSec = date.Millisecond;

            var midnight = date
                .AddHours(-hr)
                .AddMinutes(-min)
                .AddSeconds(-sec)
                .AddMilliseconds(-milliSec);

            var latest = midnight
                .AddHours(23)
                .AddMinutes(59)
                .AddSeconds(59);

            return latest;
        }
    }
}
