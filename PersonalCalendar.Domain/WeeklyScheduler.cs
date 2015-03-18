using PersonalCalendar.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCalendar.Domain
{
    public class WeeklyScheduler : Scheduler
    {
        public WeeklyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

        public override IEnumerable<DateTime> GetOccurences(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
                throw new ArgumentException("startDateTime cannot be later than endDateTime");

             List<DateTime> occurences = new List<DateTime>();

             if (_freqSubtype == FrequencySubtype.None)
             {
                 for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddDays(7 * _interval))
                 {
                     occurences.Add(dateTime);
                 }
             }
             else
             {
                 IEnumerable<DayOfWeek> daysOfWeek = GetDaysOfWeekFromFrequencySubtype();

                 foreach (var dayOfWeek in daysOfWeek)
                 {
                     DateTime startWeekday = startDateTime.GetNextWeekday(dayOfWeek);

                     for (DateTime dateTime = startWeekday; dateTime < endDateTime; dateTime = dateTime.AddDays(7 * _interval))
                     {
                         occurences.Add(dateTime);
                     }
                 }
             }

             return occurences;
         }

        public DateTime GetOccurenceDateTime(DateTime startDateTime, int occurencesCount)
        {
            if (occurencesCount < 1)
                throw new ArgumentOutOfRangeException("occurencesCount");

            DateTime dateTime = startDateTime;

            if (_freqSubtype == FrequencySubtype.None)
            {
                int occurence = 1;

                while(occurence < occurencesCount)
                {
                    dateTime = dateTime.AddDays(7 * _interval);
                    occurence++;
                }
            }
            else
            {
                IEnumerable<DayOfWeek> daysOfWeek = GetDaysOfWeekFromFrequencySubtype();
                
                List<DayOfWeek> sortedDaysOfWeek = new List<DayOfWeek>();

                for(int i = 0; i < 7; i++)
                {
                    DayOfWeek day = startDateTime.AddDays(i).DayOfWeek;

                    if (daysOfWeek.Any(d => d == day))
                        sortedDaysOfWeek.Add(day);
                }

                daysOfWeek = sortedDaysOfWeek;

                int occurence = 0;

                while(occurence < occurencesCount)
                {
                    foreach (var dayOfWeek in daysOfWeek)
                    {
                        dateTime = dateTime.GetNextWeekday(dayOfWeek);
                        occurence++;

                        if (occurence == occurencesCount)
                            break;
                    }

                    if (occurence < occurencesCount)
                        dateTime = startDateTime.GetNextWeekday(daysOfWeek.ElementAt(0)).AddDays(7 * _interval);
                }
            }

            return dateTime;
        }

        private IEnumerable<DayOfWeek> GetDaysOfWeekFromFrequencySubtype()
        {
            List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();

            if ((_freqSubtype & FrequencySubtype.Monday) == FrequencySubtype.Monday)
                daysOfWeek.Add(DayOfWeek.Monday);

            if ((_freqSubtype & FrequencySubtype.Tuesday) == FrequencySubtype.Tuesday)
                daysOfWeek.Add(DayOfWeek.Tuesday);

            if ((_freqSubtype & FrequencySubtype.Wednesday) == FrequencySubtype.Wednesday)
                daysOfWeek.Add(DayOfWeek.Wednesday);

            if ((_freqSubtype & FrequencySubtype.Thursday) == FrequencySubtype.Thursday)
                daysOfWeek.Add(DayOfWeek.Thursday);

            if ((_freqSubtype & FrequencySubtype.Friday) == FrequencySubtype.Friday)
                daysOfWeek.Add(DayOfWeek.Friday);

            if ((_freqSubtype & FrequencySubtype.Saturday) == FrequencySubtype.Saturday)
                daysOfWeek.Add(DayOfWeek.Saturday);

            if ((_freqSubtype & FrequencySubtype.Sunday) == FrequencySubtype.Sunday)
                daysOfWeek.Add(DayOfWeek.Sunday);

            return daysOfWeek;
        }
    }
}
