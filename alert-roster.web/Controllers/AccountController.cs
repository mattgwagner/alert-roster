using alert_roster.web.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alert_roster.web.Controllers
{
    [Authorize(Users = Authentication.ReadWriteRole)]
    public class AccountController : Controller
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly AlertRosterDbContext db = new AlertRosterDbContext();

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Subscriptions()
        {
            return View(db.Users.OrderBy(u => u.Name).ToList());
        }

        [HttpGet, AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(String password)
        {
            Authentication.Authenticate(password);

            return RedirectToAction("Index", "Home");
        }

        public ActionResult Logout()
        {
            Authentication.Logout();

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public ActionResult Subscription(int? ID)
        {
            return View(db.Users.Find(ID));
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Subscription(User user)
        {
            // Add/Update

            var model = db.Users.Find(user.ID);

            if (model == null)
            {
                model = db.Users.Create();
                db.Entry(model).State = System.Data.Entity.EntityState.Added;
            }

            model.Name = user.Name;
            model.EmailAddress = user.EmailAddress;
            model.EmailEnabled = user.EmailEnabled;
            model.PhoneNumber = user.PhoneNumber;
            model.SMSEnabled = user.SMSEnabled;

            db.SaveChanges();

            TempData["Message"] = "Subscription updated!";

            return RedirectToAction("Subscriptions");
        }

        // For now, unsubscribe is handled through MailGun's injected links
        // We will still try and send the notifications but they won't make it through

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Unsubscribe(int ID)
        {
            var user = db.Users.Find(ID);

            db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
            db.SaveChanges();

            TempData["Message"] = "Successfully unsubscribed!";

            return RedirectToAction("Subscriptions");
        }
    }
}