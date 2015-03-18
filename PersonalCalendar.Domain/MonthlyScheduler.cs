using PersonalCalendar.Infrastructure;
using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class MonthlyScheduler : Scheduler
    {
         public MonthlyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

         public override IEnumerable<DateTime> GetOccurrences(DateTime startDateTime, DateTime endDateTime)
         {
             if (startDateTime > endDateTime)
                 throw new ArgumentException("startDateTime cannot be later than endDateTime");

             List<DateTime> occurrences = new List<DateTime>();

             int dayOfWeekNumber = (int)Math.Ceiling(startDateTime.Day / 7.0);
             DayOfWeek weekDay = startDateTime.DayOfWeek;

             for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddMonths(_interval))
             {
                 if (_freqSubtype == FrequencySubtype.DayOfTheWeek)
                 {
                     DateTime month = new DateTime(dateTime.Year, dateTime.Month, 1);
                     DateTime occurrence = month.GetNextNthWeekdayOfTheMonth(dayOfWeekNumber, weekDay);

                     occurrences.Add(occurrence.Add(dateTime.TimeOfDay));
                 }
                 else
                 {
                     occurrences.Add(dateTime);
                 }
             }

             return occurrences;
         }
    }
}
