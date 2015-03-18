using PersonalCalendar.Infrastructure;
using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class MonthlyScheduler : Scheduler
    {
         public MonthlyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

         public override IEnumerable<DateTime> GetOccurences(DateTime startDateTime, DateTime endDateTime)
         {
             List<DateTime> occurences = new List<DateTime>();

             int dayOfWeekNumber = (int)Math.Ceiling(startDateTime.Day / 7.0);
             DayOfWeek weekDay = startDateTime.DayOfWeek;

             for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddMonths(_interval))
             {
                 if (_freqSubtype == FrequencySubtype.DayOfTheWeek)
                 {
                     DateTime month = new DateTime(dateTime.Year, dateTime.Month, 1);
                     DateTime occurence = month.GetNextNthWeekdayOfTheMonth(dayOfWeekNumber, weekDay);

                     occurences.Add(occurence.Add(dateTime.TimeOfDay));
                 }
                 else
                 {
                     occurences.Add(dateTime);
                 }
             }

             return occurences;
         }
    }
}
