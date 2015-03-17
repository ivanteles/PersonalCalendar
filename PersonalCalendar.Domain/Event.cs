using System;
using System.Collections;
using System.Collections.Generic;

namespace PersonalCalendar.Domain
{
    public class Event : ICloneable
    {
        public int Id { get; set; }
        public int CalendarId { get; set; }
        public string Title { get; set; }
        public string Location { get; set; }
        public string Description { get; set; }
        public string Color { get; set; }
        public DateTime StartDateUTC { get; set; }
        public DateTime EndDateUTC { get; set; }
        public DateTime? SeriesEndDateUTC { get; set; }
        public FrequencyType FreqType { get; set; }
        public FrequencySubtype FreqSubtype { get; set; }
        public int FreqInterval { get; set; }

        public Calendar Calendar { get; set; }

        public IEnumerable<DateTime> GetSeriesOccurences(DateTime endDateUTC)
        {
            List<DateTime> occurences = new List<DateTime>();

            DateTime eventEndDateTime = SeriesEndDateUTC.HasValue 
                                        ? (SeriesEndDateUTC.Value < endDateUTC ? SeriesEndDateUTC.Value : endDateUTC) 
                                        : endDateUTC;

            switch(FreqType)
            {
                case FrequencyType.Daily:
                    for (DateTime dateTime = StartDateUTC; dateTime < eventEndDateTime; dateTime = dateTime.AddDays(FreqInterval))
                    {
                        occurences.Add(dateTime);
                    }
                    break;

                case FrequencyType.Weekly:
                    if(FreqSubtype == FrequencySubtype.None)
                    {
                        for (DateTime dateTime = StartDateUTC; dateTime < eventEndDateTime; dateTime = dateTime.AddDays(7 * FreqInterval))
                        {
                            occurences.Add(dateTime);
                        }
                    }
                    else
                    {
                        List<DayOfWeek> daysOfWeek = new List<DayOfWeek>();
                       
                        if((FreqSubtype & FrequencySubtype.Monday) == FrequencySubtype.Monday)
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

                        foreach(var dayOfWeek in daysOfWeek)
                        {
                            DateTime startWeekday = GetNextWeekday(StartDateUTC, dayOfWeek);

                            for (DateTime dateTime = startWeekday; dateTime < eventEndDateTime; dateTime = dateTime.AddDays(7 * FreqInterval))
                            {
                                occurences.Add(dateTime);
                            }
                        }
                    }

                    break;

                case FrequencyType.Monthly:
                    int dayOfWeekNumber = (int)Math.Ceiling(StartDateUTC.Day / 7.0);
                    DayOfWeek weekDay = StartDateUTC.DayOfWeek;

                    for (DateTime dateTime = StartDateUTC; dateTime < eventEndDateTime; dateTime = dateTime.AddMonths(FreqInterval))
                    {
                        if(FreqSubtype == FrequencySubtype.DayOfTheWeek)
                        {
                            DateTime month = new DateTime(dateTime.Year, dateTime.Month, 1);
                            DateTime occurence = GetNthWeekDayOfTheMonth(month, dayOfWeekNumber, weekDay);
                            
                            occurences.Add(occurence.Add(dateTime.TimeOfDay));
                        }
                        else
                        {
                            occurences.Add(dateTime);
                        }
                    }

                    break;

                case FrequencyType.Yearly:

                    for (DateTime dateTime = StartDateUTC; dateTime < eventEndDateTime; dateTime = dateTime.AddYears(FreqInterval))
                    {
                        occurences.Add(dateTime);
                    }

                    break;

                case FrequencyType.EveryEvenWeekday:
                case FrequencyType.EveryOddWeekday:
                case FrequencyType.EveryWeekday:
                    throw new NotImplementedException();
                   
            }

            return occurences;
        }

        // TODO: Move as extension method.
        private DateTime GetNextWeekday(DateTime start, DayOfWeek day)
        {
            int daysToAdd = ((int)day - (int)start.DayOfWeek + 7) % 7;
            
            return start.AddDays(daysToAdd);
        }


        private DateTime GetNthWeekDayOfTheMonth(DateTime date, int nthWeekDay, DayOfWeek dayOfWeek)
        {
            DateTime d = date.AddDays((dayOfWeek < date.DayOfWeek ? 7 : 0) + dayOfWeek - date.DayOfWeek);

            return d.AddDays((nthWeekDay - 1) * 7);
        }

        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
