using alert_roster.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alert_roster.web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (var db = new AlertRosterDbContext())
            {
                var messages = from m in db.Messages
                               orderby m.PostedDate descending
                               select m;

                return View(messages);
            }
        }

        public ActionResult About()
        {
            return View();
        }
    }
}