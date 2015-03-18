using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class DailyScheduler : Scheduler
    {
        public DailyScheduler(FrequencySubtype freqSubtype, int interval)
            : base(freqSubtype, interval) { }

        public override IEnumerable<DateTime> GetOccurences(DateTime startDateTime, DateTime endDateTime)
        {
            if (startDateTime > endDateTime)
                throw new ArgumentException("startDateTime cannot be later than endDateTime");

            List<DateTime> occurences = new List<DateTime>();

            for (DateTime dateTime = startDateTime; dateTime < endDateTime; dateTime = dateTime.AddDays(_interval))
            {
                occurences.Add(dateTime);
            }

            return occurences;
        }
    }
}
