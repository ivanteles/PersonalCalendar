using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public abstract class Scheduler
    {
        protected FrequencySubtype _freqSubtype;
        protected int _interval;

        public Scheduler(FrequencySubtype freqSubtype, int interval)
        {
            if (interval < 1)
                throw new ArgumentOutOfRangeException("interval");

            _freqSubtype = freqSubtype;
            _interval = interval;
        }

        public abstract IEnumerable<DateTime> GetOccurences(DateTime startDateTime, DateTime endDateTime);
    }
}
