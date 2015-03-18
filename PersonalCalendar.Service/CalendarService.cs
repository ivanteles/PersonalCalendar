using PersonalCalendar.Data;
using PersonalCalendar.Domain;

namespace PersonalCalendar.Service
{
    public interface ICalendarService : ICrudService<Calendar>
    {
    }

    public class CalendarService : CrudService<Calendar>, ICalendarService
    {
        public CalendarService(CalendarDB context) 
            : base(context) { }

    }
}
