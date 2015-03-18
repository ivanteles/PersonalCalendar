using PersonalCalendar.Data;
using PersonalCalendar.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace PersonalCalendar.Service
{
    public interface ICalendarService
    {
        IEnumerable<Calendar> GetAll(Expression<Func<Calendar, bool>> predicate);

        Calendar Find(int id);
    }

    public class CalendarService : ICalendarService
    {
        protected readonly CalendarDB _calendarDB;

        public CalendarService(CalendarDB context)
        {
            _calendarDB = context;
        }

        public IEnumerable<Calendar> GetAll(Expression<Func<Calendar, bool>> predicate)
        {
            return _calendarDB.Calendars.Where(predicate).ToList();
        }

        public Calendar Find(int id)
        {
            return _calendarDB.Calendars.Find(id);
        }
    }
}
