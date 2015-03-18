using AutoMapper;
using PersonalCalendar.Domain;
using PersonalCalendar.Service;
using PersonalCalendar.Web.ViewModels.Calendars;
using System.Collections.Generic;
using System.Web.Mvc;

namespace PersonalCalendar.Web.Controllers
{
    public class CalendarsController : Controller
    {
        protected readonly ICalendarService _service;

        public CalendarsController(ICalendarService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult Index()
        {
            IEnumerable<Calendar> calendars = _service.GetAll(c => c.UserId == 1);

            var calendarViewModels = Mapper.Map<IEnumerable<CalendarViewModel>>(calendars);

            return View(calendarViewModels);
        }

        [HttpGet]
        public ActionResult Details(int id)
        {
            Calendar calendar = _service.Find(id);

            var calendarViewModel = Mapper.Map<CalendarViewModel>(calendar);

            return View(calendarViewModel);
        }
    }
}