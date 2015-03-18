using System;

namespace PersonalCalendar.Web.ViewModels.Events
{
    public class EventDetailsViewModel
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Location { get; set; }

        public string Description { get; set; }

        public string Color { get; set; }

        public DateTime StartDateTime { get; set; }

        public DateTime EndDateTime { get; set; }
    }
}