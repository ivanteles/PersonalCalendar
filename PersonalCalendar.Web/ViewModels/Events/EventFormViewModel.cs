using PersonalCalendar.Domain;
using System;

namespace PersonalCalendar.Web.ViewModels.Events
{
    public class EventFormViewModel
    {
        public int Id { get; set; }

        public int CalendarId { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }

        public FrequencyType FreqType { get; set; }

        public int FreqInterval { get; set; }
    }
}