using System.Web.Mvc;
using System.Web.Routing;

namespace PersonalCalendar.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            UnityConfig.RegisterComponents();
            AutoMapperConfig.RegisterMappings();
            BundleConfig.RegisterBundles();
        }
    }
}
