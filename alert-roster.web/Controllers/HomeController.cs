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
        public ActionResult Login(String password)
        {
            if (ReadOnlyPassord == password)
            {
                FormsAuthentication.SetAuthCookie("ReadOnly", true);
            }
            else if (ReadWritePassword == password)
            {
                FormsAuthentication.SetAuthCookie("ReadWrite", true);
            }
            else
            {
                // Miss, send back to login for now
                return RedirectToAction("Login");
            }

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