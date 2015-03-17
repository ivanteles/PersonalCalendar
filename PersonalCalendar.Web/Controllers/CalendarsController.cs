using PersonalCalendar.Service;
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

        public ActionResult Index()
        {
            var calendars = _service.GetAll(x => x.Id > 0);

            return View(calendars);
        }
    }
}