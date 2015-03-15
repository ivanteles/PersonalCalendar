using System;

namespace PersonalCalendar.Domain
{
    [Flags]
    public enum FrequencySubtype
    {
        None = 0,
        Monday = 1,
        Tuesday = 2,
        Wednesday = 4,
        Thursday = 8,
        Friday = 16,
        Saturday = 32,
        Sunday = 64,
        DayOfTheMonth = 128,
        DayOfTheWeek = 256
    }
}
