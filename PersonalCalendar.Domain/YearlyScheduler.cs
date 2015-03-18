using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class YearlyScheduler : Scheduler
    {
        public YearlyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

        public override IEnumerable<DateTime> GetOccurrences(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
                throw new ArgumentException("startDateTime cannot be later than endDateTime");

            List<DateTime> occurrences = new List<DateTime>();

            for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddYears(_interval))
            {
                occurrences.Add(dateTime);
            }

            return occurrences;
        }
    }
}
