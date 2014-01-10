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
        private String ReadOnlyPassord { get { return ConfigurationManager.AppSettings["Password.ReadOnly"]; } }

        private String ReadWritePassword { get { return ConfigurationManager.AppSettings["Password.ReadWrite"]; } }

        private const String ReadOnlyUser = "ReadOnly";

        private const String ReadWriteUser = "ReadWrite";

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
            if (ReadOnlyPassord == password)
            {
                FormsAuthentication.SetAuthCookie(ReadOnlyUser, true);
            }
            else if (ReadWritePassword == password)
            {
                FormsAuthentication.SetAuthCookie(ReadWriteUser, true);
            }
            else
            {
                return View("Error");
            }

            if (String.IsNullOrWhiteSpace(ReturnUrl))
            {
                return RedirectToAction("Index");
            }

            return Redirect(ReturnUrl);
        }

        [Authorize(Users = ReadWriteUser)]
        public ActionResult New()
        {
            return View();
        }

        [Authorize(Users = ReadWriteUser), HttpPost, ValidateAntiForgeryToken]
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