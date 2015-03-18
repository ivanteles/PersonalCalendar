using System;

namespace PersonalCalendar.Domain
{
    public enum FrequencyType
    {
        OnlyOnce = 0,
        Daily = 1,
        EveryWeekday = 2,
        EveryOddWeekday = 4,
        EveryEvenWeekday = 8,
        Weekly = 16,
        Monthly = 32,
        Yearly = 64
    }
}
