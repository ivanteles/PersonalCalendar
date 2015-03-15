using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;

namespace PersonalCalendar.Service
{
    public class EventService
    {
        protected readonly CalendarDB _calendarDB;

        public EventService(CalendarDB context)
        {
            _calendarDB = context;
        }

        public IEnumerable<Event> GetAllForCalendar(long calendarId)
        {
            return _calendarDB.Events.Where(e => e.CalendarId == calendarId);
        }

        public IEnumerable<Event> GetAllForCalendar(long calendarId, DateTime dateUTC)
        {
            return GetAllForCalendar(calendarId, dateUTC, dateUTC);
        }

        public IEnumerable<Event> GetAllForCalendar(long calendarId, DateTime startDateUTC, DateTime endDateUTC)
        {
            return _calendarDB.Events;
        }
    }
}
