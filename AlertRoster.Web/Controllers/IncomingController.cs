using AlertRoster.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.TwiML;

namespace AlertRoster.Web.Controllers
{
    public class IncomingController : TwilioController
    {
        // Messages: Subscribe, Unsubscribe, Content, Add User, List Users?

        private readonly Database db;

        public IncomingController(Database db)
        {
            this.db = db;
        }

        [HttpPost(template: "/api/incoming"), AllowAnonymous]
        public virtual async Task<IActionResult> Handle(SmsRequest request)
        {
            var to = request.To;

            var from = request.From;

            var content = request.Body;

            var response = new MessagingResponse();

            var group =
                await db
                .Groups
                .Where(_ => _.PhoneNumber == to)
                .SingleOrDefaultAsync();

            if (group == null)
            {
                response.Message("Sorry, this group does not exist!");

                return TwiML(response);
            }

            var member =
                group
                .Members
                .Where(_ => _.Member.PhoneNumber == from)
                .SingleOrDefault();

            // TODO Twilio can handle industry-standard unsubscribe requests, research this.

            if (member != null)
            {
                // Hmm, not currently in the group, check if they exist elsewhere
                // They could be in multiple other groups, so we use First here

                member =
                    await db
                    .MemberGroups
                    .Where(_ => _.Member.PhoneNumber == from)
                    .FirstOrDefaultAsync();

                if (member == null)
                {
                    // Creating a new recipient that we haven't seen before

                    // FIXME We're assuming right now that the first message will be their display name

                    var tracker = db.Members.Add(new Member(from, content));

                    member = new MemberGroup(tracker.Entity.Id, group.Id);

                    await db.SaveChangesAsync();
                }

                response.Message($"Thank you for subscribing to {group.DisplayName}!");

                group.Members.Add(new MemberGroup(member.MemberId, group.Id));

                group.Messages.Add(new Message(group.Id, member.MemberId, "Joined the group."));
            }
            else
            {
                db.Messages.Add(new Message(group.Id, member.MemberId, content));
            }

            await db.SaveChangesAsync();

            return TwiML(response);
        }
    }
}