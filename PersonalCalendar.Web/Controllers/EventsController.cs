using PersonalCalendar.Service;
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

        public ActionResult Index()
        {
            return View();
        }
    }
}