using PersonalCalendar.Service;
using PersonalCalendar.Web.ViewModels.Events;
using System.Web.Mvc;

namespace PersonalCalendar.Web.Controllers
{
    public class EventsController : Controller
    {
        protected readonly IEventService _service;

        public EventsController(IEventService service)
        {
            _service = service;
        }

        [HttpGet]
        public ActionResult New(int calendarId)
        {
            var eventFormViewModel = new EventFormViewModel { CalendarId = calendarId };

            return View(eventFormViewModel);
        }

        [HttpPost]
        public ActionResult Create(EventFormViewModel eventFormViewModel)
        {
            return RedirectToAction("Details", "Calendars", new { id = eventFormViewModel.CalendarId });
        }
    }
}