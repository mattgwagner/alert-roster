using AlertRoster.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

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

        [HttpGet("/api/groups/{id:int?}")]
        public dynamic GetGroups(int? id)
        {
            return
                db
                .Groups
                .Where(group => !id.HasValue || group.Id == id)
                .Select(group => new
                {
                    group.Id,
                    group.DisplayName,
                    group.PhoneNumber,
                    members =
                        group
                        .Members
                        .Select(member => new
                        {
                            member.MemberId,
                            member.Member.DisplayName,
                            member.Role
                        })
                });
        }

        [HttpPost("/api/groups")]
        public async Task<dynamic> CreateGroup([FromBody]Group group)
        {
            var result = db.Groups.Add(new Group(group.DisplayName)
            {
                PhoneNumber = group.PhoneNumber
            });

            await db.SaveChangesAsync();

            // TODO Go get phone #

            return GetGroups(result.Entity.Id);
        }

        [HttpDelete("/api/groups/{id}")]
        public async Task<IActionResult> RemoveGroup(int id)
        {
            var group =
                await db
                .Groups
                .SingleOrDefaultAsync(_ => _.Id == id);

            // Perhaps we can mark this archived instead?
            // Maybe reject should be only admins can send?

            // group.Replies = Group.ReplyMode.Reject;

            db.Groups.Remove(group);

            await db.SaveChangesAsync();

            return Ok();
        }

        [HttpGet("/api/groups/{id}/messages")]
        public async Task<dynamic> GetMessages(int id, DateTime? since)
        {
            return
                await db
                .Messages
                .Where(msg => msg.GroupId == id)
                .Where(msg => !since.HasValue || msg.Timestamp >= since)
                .Select(msg => new
                {
                    msg.Id,
                    msg.Timestamp,
                    msg.Status,
                    msg.SenderId,
                    msg.DisplayContent
                })
                .ToListAsync();
        }

        [HttpGet("/api/groups/{id}/members")]
        public async Task<dynamic> GetMembers(int id)
        {
            return
                await db
                .MemberGroups
                .Where(group => group.GroupId == id)
                .Select(group => group.Member)
                .Select(member => new
                {
                    member.Id,
                    member.DisplayName
                })
                .ToListAsync();
        }

        [HttpPost("/api/groups/{id}/members")]
        public async Task<IActionResult> AddMember(int id, [FromBody]String phone, [FromBody]String displayName)
        {
            // TODO Find or create user, add them to the group

            await db.SaveChangesAsync();

            return Accepted();
        }
    }
}