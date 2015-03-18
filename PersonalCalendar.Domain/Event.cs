using PersonalCalendar.Infrastructure;
using System;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class Event : Entity, ICloneable
    {
        public int CalendarId { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public DateTime StartDateTimeUTC { get; set; }

        public DateTime EndDateTimeUTC { get; set; }

        public DateTime? SeriesEndDateUTC { get; set; }

        public FrequencyType FreqType { get; set; }

        public FrequencySubtype FreqSubtype { get; set; }

        public int FreqInterval { get; set; }

        public Calendar Calendar { get; set; }

        // TODO: Probably Open-Closed principle violation!
        public IEnumerable<DateTime> GetSeriesOccurences(DateTime endDateTimeUTC)
        {
            IEnumerable<DateTime> occurences = new List<DateTime>();

            DateTime eventEndDateTime = SeriesEndDateUTC.HasValue 
                                        ? (SeriesEndDateUTC.Value < endDateTimeUTC ? SeriesEndDateUTC.Value : endDateTimeUTC) 
                                        : endDateTimeUTC;

            switch(FreqType)
            {
                case FrequencyType.Daily:
                    occurences = GetDailyRecurringOccurences(eventEndDateTime);
                    break;

                case FrequencyType.Weekly:
                    occurences = GetWeeklyRecurringOccurences(eventEndDateTime);
                    break;

                case FrequencyType.Monthly:
                    occurences = GetMonthlyRecurringOccurences(eventEndDateTime);
                    break;

                case FrequencyType.Yearly:
                    occurences = GetYearlyRecurringOccurences(eventEndDateTime);
                    break;

                case FrequencyType.EveryEvenWeekday:
                case FrequencyType.EveryOddWeekday:
                case FrequencyType.EveryWeekday:
                    throw new NotImplementedException();
            }

            return occurences;
        }

        private IEnumerable<DateTime> GetDailyRecurringOccurences(DateTime endDateTimeUTC)
        {
            List<DateTime> occurences = new List<DateTime>();

            for (DateTime dateTime = StartDateTimeUTC; dateTime < endDateTimeUTC; dateTime = dateTime.AddDays(FreqInterval))
            {
                occurences.Add(dateTime);
            }

            return occurences;
        }

        private IEnumerable<DateTime> GetWeeklyRecurringOccurences(DateTime endDateTimeUTC)
        {
            List<DateTime> occurences = new List<DateTime>();

            if (FreqSubtype == FrequencySubtype.None)
            {
                for (DateTime dateTime = StartDateTimeUTC; dateTime < endDateTimeUTC; dateTime = dateTime.AddDays(7 * FreqInterval))
                {
                    occurences.Add(dateTime);
                }
            }
            else
            {
                List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();

                if ((FreqSubtype & FrequencySubtype.Monday) == FrequencySubtype.Monday)
                    daysOfWeek.Add(DayOfWeek.Monday);

                if ((FreqSubtype & FrequencySubtype.Tuesday) == FrequencySubtype.Tuesday)
                    daysOfWeek.Add(DayOfWeek.Tuesday);

                if ((FreqSubtype & FrequencySubtype.Wednesday) == FrequencySubtype.Wednesday)
                    daysOfWeek.Add(DayOfWeek.Wednesday);

                if ((FreqSubtype & FrequencySubtype.Thursday) == FrequencySubtype.Thursday)
                    daysOfWeek.Add(DayOfWeek.Thursday);

                if ((FreqSubtype & FrequencySubtype.Friday) == FrequencySubtype.Friday)
                    daysOfWeek.Add(DayOfWeek.Friday);

                if ((FreqSubtype & FrequencySubtype.Saturday) == FrequencySubtype.Saturday)
                    daysOfWeek.Add(DayOfWeek.Saturday);

                if ((FreqSubtype & FrequencySubtype.Sunday) == FrequencySubtype.Sunday)
                    daysOfWeek.Add(DayOfWeek.Sunday);

                foreach (var dayOfWeek in daysOfWeek)
                {
                    DateTime startWeekday = StartDateTimeUTC.GetNextWeekday(dayOfWeek);

                    for (DateTime dateTime = startWeekday; dateTime < endDateTimeUTC; dateTime = dateTime.AddDays(7 * FreqInterval))
                    {
                        occurences.Add(dateTime);
                    }
                }
            }

            return occurences;
        }

        private IEnumerable<DateTime> GetMonthlyRecurringOccurences(DateTime endDateTimeUTC)
        {
            List<DateTime> occurences = new List<DateTime>();

            int dayOfWeekNumber = (int)Math.Ceiling(StartDateTimeUTC.Day / 7.0);
            DayOfWeek weekDay = StartDateTimeUTC.DayOfWeek;

            for (DateTime dateTime = StartDateTimeUTC; dateTime < endDateTimeUTC; dateTime = dateTime.AddMonths(FreqInterval))
            {
                if (FreqSubtype == FrequencySubtype.DayOfTheWeek)
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

        private IEnumerable<DateTime> GetYearlyRecurringOccurences(DateTime endDateTimeUTC)
        {
            List<DateTime> occurences = new List<DateTime>();

            for (DateTime dateTime = StartDateTimeUTC; dateTime < endDateTimeUTC; dateTime = dateTime.AddYears(FreqInterval))
            {
                occurences.Add(dateTime);
            }

            return occurences;
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
