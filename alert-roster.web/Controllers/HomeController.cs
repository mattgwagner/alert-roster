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
                var messages = db.Messages.OrderByDescending(m => m.PostedDate).Take(10).ToList();

                return View(messages);
            }
        }

        public ActionResult About()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Post()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Post(Message message)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AlertRosterDbContext())
                {
                    db.Messages.Add(message);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(message);
        }
    }
}