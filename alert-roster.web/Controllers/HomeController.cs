using alert_roster.web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alert_roster.web.Controllers
{
    [Authorize]
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

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        public ActionResult Login(String password)
        {
            // TODO Check password

            return RedirectToAction("Index");
        }

        public ActionResult New()
        {
            return View();
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "Content")]Message message)
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