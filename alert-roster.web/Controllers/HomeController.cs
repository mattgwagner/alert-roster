using alert_roster.web.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace alert_roster.web.Controllers
{
    public class HomeController : Controller
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly AlertRosterDbContext db = new AlertRosterDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            var messages = db.Messages.OrderByDescending(m => m.PostedDate).Take(10).ToList();

            return View(messages);
        }

        [HttpGet, AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
    }
}