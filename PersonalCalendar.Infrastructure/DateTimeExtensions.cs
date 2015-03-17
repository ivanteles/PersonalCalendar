using System;

namespace PersonalCalendar.Infrastructure
{
    public static class DateTimeExtensions
    {
        public static DateTime GetNextWeekday(this DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;

            return start.AddDays(daysToAdd);
        }

        public static DateTime GetNextNthWeekdayOfTheMonth(this DateTime start, int nthWeekDay, DayOfWeek dayOfWeek)
        {
            DateTime d = start.AddDays((dayOfWeek < start.DayOfWeek ? 7 : 0) + dayOfWeek - start.DayOfWeek);

            return d.AddDays((nthWeekDay - 1) * 7);
        }
    }
}
