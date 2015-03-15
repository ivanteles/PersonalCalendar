using System;

namespace PersonalCalendar.Domain
{
    public class Event
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
    }
}
