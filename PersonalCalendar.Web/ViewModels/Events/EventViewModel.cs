using System;
using System.Collections.Generic;

namespace PersonalCalendar.Web.ViewModels.Events
{
    public class EventViewModel
    {
        public DateTime OccurenceDateTime { get; set; }

        public IEnumerable<EventDetailsViewModel> Details { get; set; }
    }
}