using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class YearlyScheduler : Scheduler
    {
        public YearlyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

        public override IEnumerable<DateTime> GetOccurences(DateTime startDateTime, DateTime endDateTime)
        {
            List<DateTime> occurences = new List<DateTime>();

            for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddYears(_interval))
            {
                occurences.Add(dateTime);
            }

            return occurences;
        }
    }
}
