using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PersonalCalendar.Service
{
    public class CalendarService
    {
        CalendarDB _calendarDB = new CalendarDB();

        public IEnumerable<Calendar> GetAll(Expression<Func<Calendar, bool>> predicate)
        {
            return _calendarDB.Calendars.AsEnumerable<Calendar>();
        }
    }
}
