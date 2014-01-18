using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace alert_roster.web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        protected void Application_Start()
        {
            ConfigureLogging();

            AreaRegistration.RegisterAllAreas();

            RegisterBundles(BundleTable.Bundles);

            RegisterRoutes(RouteTable.Routes);

            GlobalFilters.Filters.Add(new HandleErrorAttribute());
            GlobalFilters.Filters.Add(new AuthorizeAttribute());
        }

        protected void Application_Error(object sender, EventArgs e)
        {
            log.ErrorException("Caught unhandled exception", Server.GetLastError());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }

        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/libs").Include(
                        "~/Scripts/moment.js",
                        "~/Scripts/jQuery-{version}.js",
                        "~/Scripts/bootstrap.js",
                        "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css"));
        }

        public static void ConfigureLogging()
        {
            var config = new NLog.Config.LoggingConfiguration();

            // TODO NLog configuration

            LogManager.Configuration = config;
        }
    }
}
