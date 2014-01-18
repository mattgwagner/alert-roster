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

                    EmailSender.Send(message.Content);

                    SMSSender.Send(message.Content);

                    TempData["Message"] = "Message posted!";
                }

                return RedirectToAction("Index");
            }

            return View(message);
        }

        public ActionResult Subscription(int? ID)
        {
            using (var db = new AlertRosterDbContext())
            {
                var subscription = db.Users.SingleOrDefault(s => s.ID == ID);

                return View(subscription);
            }
        }

        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Subscription(User user)
        {
            using (var db = new AlertRosterDbContext())
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
        }

        [Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Subscriptions()
        {
            using (var db = new AlertRosterDbContext())
            {
                return View(db.Users.ToList());
            }
        }

        // For now, unsubscribe is handled through MailGun's injected links
        // We will still try and send the notifications but they won't make it through

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

        [HttpPost]
        public ActionResult IncomingMessage(String To, String From, String Body)
        {
            // Check FROM to see if they're a group member

            // Do the requested action

            // respond with confirmation message if required

            return Content(@"<response></response>", "text/xml");
        }
    }
}