using AutoMapper;
using PersonalCalendar.Domain;
using PersonalCalendar.Web.ViewModels.Calendars;
using PersonalCalendar.Web.ViewModels.Events;

namespace PersonalCalendar.Web
{
    public class AutoMapperConfig
    {
        public static void RegisterMappings()
        {
            Mapper.CreateMap<Calendar, CalendarViewModel>();
            Mapper.CreateMap<Event, EventDetailsViewModel>()
                .ForMember(dst => dst.StartDateTime, opt => opt.MapFrom(src => src.StartDateTimeUTC.ToLocalTime()))
                .ForMember(dst => dst.EndDateTime, opt => opt.MapFrom(src => src.EndDateTimeUTC.ToLocalTime()));
        }
    }
}