using AlertRoster.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;

namespace AlertRoster.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly Database db;

        public HomeController(Database db)
        {
            this.db = db;
        }

        [HttpGet(template: "/api"), AllowAnonymous]
        public dynamic HandleGet()
        {
            return new
            {
                Machine = Environment.MachineName,
                Timestamp = DateTimeOffset.UtcNow
            };
        }

        // Show Groups User Can See

        // View Group Details

        // Add/Remove Groups

        // Add/Remove Members
    }
}