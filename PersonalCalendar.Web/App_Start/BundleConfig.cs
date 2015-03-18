using System.Web.Optimization;

namespace PersonalCalendar.Web
{
	public class BundleConfig
	{
		public static void RegisterBundles()
		{
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/jquery").Include("~/Scripts/jquery-1.9.0.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/plugins").Include("~/Scripts/jquery.datetimepicker.js"));
            BundleTable.Bundles.Add(new ScriptBundle("~/bundles/app").Include("~/Scripts/App/events-new.js"));

			BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap").Include("~/Content/bootstrap.css"));
			BundleTable.Bundles.Add(new StyleBundle("~/Content/bootstrap-theme").Include("~/Content/bootstrap-theme.css"));
            BundleTable.Bundles.Add(new StyleBundle("~/Content/plugins").Include("~/Content/jquery.datetimepicker.css"));
		}
	}
}
