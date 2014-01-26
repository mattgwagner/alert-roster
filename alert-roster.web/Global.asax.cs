using NLog;
using NLog.Config;
using NLog.Targets;
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

            log.Info("Application started.");
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
            // TODO NLog configuration

            var config = new NLog.Config.LoggingConfiguration();

#if (!DEBUG)
            var databaseTarget = new DatabaseTarget
            {
                ConnectionStringName = "MainDb",
                CommandText = ""
            };

            var dbRule = new LoggingRule("*", LogLevel.Warn, databaseTarget);

            config.LoggingRules.Add(dbRule);
#endif

            var fileTarget = new FileTarget
            {
                CreateDirs = true,
                FileName = "Log.txt"
            };

            config.AddTarget("fileTarget", fileTarget);

            var fileRule = new LoggingRule("*", LogLevel.Debug, fileTarget);

            config.LoggingRules.Add(fileRule);

            LogManager.Configuration = config;
        }
    }
}
