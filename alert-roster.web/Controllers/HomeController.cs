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
        public ActionResult About()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous, HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(String password)
        {
            Authentication.Authenticate(password);

            return RedirectToAction("Index");
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

                    new EmailSender().Send(message.Content);

                    TempData["Message"] = "Message posted!";
                }

                return RedirectToAction("Index");
            }

            return View(message);
        }

        public ActionResult Subscribe()
        {
            return View();
        }

        // For now, unsubscribe is handled through MailGun's injected links
        // We will still try and send the notifications but they won't make it through

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Subscribe(User user)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AlertRosterDbContext())
                {
                    if (db.Users.Any(u => u.EmailAddress == user.EmailAddress))
                    {
                        TempData["Message"] = "Already subscribed!";
                        return View("Index");
                    }

                    db.Users.Add(user);

                    db.SaveChanges();

                    TempData["Message"] = "Successfully subscribed!";
                }

                return RedirectToAction("Index");
            }

            return View(user);
        }

        [Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Subscriptions()
        {
            using (var db = new AlertRosterDbContext())
            {
                return View(db.Users.ToList());
            }
        }

        public ActionResult Unsubscribe(int ID)
        {
            using (var db = new AlertRosterDbContext())
            {
                var user = db.Users.Find(ID);

                db.Entry(user).State = System.Data.Entity.EntityState.Deleted;
                db.SaveChanges();

                TempData["Message"] = "Successfully unsubscribed!";

                return RedirectToAction("Subscriptions");
            }
        }
    }
}