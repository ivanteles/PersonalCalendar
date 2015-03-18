using AutoMapper;
using PersonalCalendar.Domain;
using PersonalCalendar.Web.ViewModels.Calendars;

namespace PersonalCalendar.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Calendar, CalendarViewModel>();
        }
    }
}