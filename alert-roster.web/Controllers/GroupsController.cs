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
    public class GroupsController : Controller
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly AlertRosterDbContext db = new AlertRosterDbContext();

        [HttpGet]
        public ActionResult Index()
        {
            return View(db.Groups.ToList());
        }

        [HttpGet]
        public ActionResult Groups()
        {
            return View(db.Groups.ToList());
        }

        [HttpGet]
        public ActionResult Group(int? ID)
        {
            return View(db.Groups.Find(ID));
        }

        [HttpPost]
        public ActionResult Group(Group group)
        {
            // TODO Create/Update group

            TempData["Message"] = "Group updated!";

            return RedirectToAction("Groups");
        }
    }
}