using alert_roster.web.Models;
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




        public ActionResult Index()
        {
            using (var db = new AlertRosterDbContext())
            {
                var messages = db.Messages.OrderByDescending(m => m.PostedDate).Take(10).ToList();

                return View(messages);
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(String password, String ReturnUrl = "")
        {
            Authentication.Authenticate(password);

            if (String.IsNullOrWhiteSpace(ReturnUrl))
            {
                return RedirectToAction("Index");
            }

            return Redirect(ReturnUrl);
        }

        [Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Users = Authentication.ReadWriteRole), HttpPost, ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "Content")]Message message)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AlertRosterDbContext())
                {
                    message.PostedDate = DateTime.UtcNow;
                    db.Messages.Add(message);

                    db.SaveChanges();
                }

                return RedirectToAction("Index");
            }

            return View(message);
        }
    }
}