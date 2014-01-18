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

        [HttpGet]
        public ActionResult Index()
        {
            using (var db = new AlertRosterDbContext())
            {
                var messages = db.Messages.OrderByDescending(m => m.PostedDate).Take(10).ToList();

                return View(messages);
            }
        }

        [HttpGet, AllowAnonymous]
        public ActionResult About()
        {
            return View();
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

            return RedirectToAction("Index");
        }

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult New()
        {
            return View();
        }

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole), HttpPost, ValidateAntiForgeryToken]
        public ActionResult New([Bind(Include = "Content")]Message message)
        {
            if (ModelState.IsValid)
            {
                using (var db = new AlertRosterDbContext())
                {
                    message.PostedDate = DateTime.UtcNow;
                    db.Messages.Add(message);

                    db.SaveChanges();

                    EmailSender.Send(db.Users.Where(u => u.EmailEnabled).Select(u => u.EmailAddress), message.Content);

                    SMSSender.Send(db.Users.Where(u => u.SMSEnabled).Select(u => u.PhoneNumber), message.Content);

                    TempData["Message"] = "Message posted!";
                }

                return RedirectToAction("Index");
            }

            return View(message);
        }

        [HttpGet]
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

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Subscriptions()
        {
            using (var db = new AlertRosterDbContext())
            {
                return View(db.Users.ToList());
            }
        }

        // For now, unsubscribe is handled through MailGun's injected links
        // We will still try and send the notifications but they won't make it through

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
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

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Groups()
        {
            using (var db = new AlertRosterDbContext())
            {
                return View(db.Groups.ToList());
            }
        }

        [HttpGet, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Group(int? ID)
        {
            using (var db = new AlertRosterDbContext())
            {
                var group = db.Groups.SingleOrDefault(g => g.ID == ID);

                return View(group);
            }
        }

        [HttpPost, Authorize(Users = Authentication.ReadWriteRole)]
        public ActionResult Group(Group group)
        {
            using (var db = new AlertRosterDbContext())
            {
                // TODO Create/Update group

                return View();
            }
        }

        [HttpPost]
        public ActionResult IncomingMessage(String To, String From, String Body)
        {
            // TODO Process incoming messages

            // For now, just shoot me an email with the content

            EmailSender.Send(new[] { "mattgwagner@gmail.com" }, String.Format("Message from {0}: {1}", From, Body));

            // Check FROM to see if they're a group member

            // Do the requested action

            // respond with confirmation message if required by adding <message>content</message> to the response

            return Content(@"<response></response>", "text/xml");
        }
    }
}