using PersonalCalendar.Service;
using System.Web.Mvc;

namespace PersonalCalendar.Web.Controllers
{
    public class CalendarsController : Controller
    {
        public ActionResult Index()
        {
            CalendarService service = new CalendarService();

            var calendars = service.GetAll(x => x.Id > 0);

            return View(calendars);
        }

        public ActionResult Details()
        {
            return View();
        }
    }
}