using alert_roster.web.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace alert_roster.web.Controllers
{
    public class MessasgesController : Controller
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly AlertRosterDbContext db = new AlertRosterDbContext();

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
                message.PostedDate = DateTime.UtcNow;
                db.Messages.Add(message);

                db.SaveChanges();

                EmailSender.Send(db.Users.Where(u => u.EmailEnabled).Select(u => u.EmailAddress), message.Content);

                SMSSender.Send(db.Users.Where(u => u.SMSEnabled).Select(u => u.PhoneNumber), message.Content);

                TempData["Message"] = "Message posted!";

                return RedirectToAction("Index");
            }

            return View(message);
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