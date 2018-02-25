using AlertRoster.Web.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using Twilio.AspNet.Common;
using Twilio.AspNet.Core;
using Twilio.Security;
using Twilio.TwiML;

namespace AlertRoster.Web.Controllers
{
    public class IncomingController : TwilioController
    {
        // Messages: Subscribe, Unsubscribe, Content, Add User, List Users?

        private readonly Database db;
        private readonly IOptions<Settings> settings;

        public IncomingController(Database db, IOptions<Settings> settings)
        {
            this.db = db;
            this.settings = settings;
        }

        [HttpPost(template: "/api/incoming"), AllowAnonymous]
        public virtual async Task<IActionResult> Handle(SmsRequest request)
        {
            var validator = new RequestValidator(settings.Value.Twilio.AuthToken);

#if RELEASE
            // TODO Request validation
#endif
            var to = request.To;

            var from = request.From;

            var content = request.Body;

            var response = new MessagingResponse();

            var group =
                await db
                .Groups
                .Include(_ => _.Members)
                .ThenInclude(_ => _.Member)
                .Include(_ => _.Messages)
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
                .Select(_ => _.Member)
                .Where(_ => _.PhoneNumber == from)
                .SingleOrDefault();

            // TODO Twilio can handle industry-standard unsubscribe requests, research this.

            if (member == null)
            {
                // Hmm, not currently in the group, check if they exist elsewhere
                // They could be in multiple other groups, so we use First here

                member =
                    await db
                    .Members
                    .Where(_ => _.PhoneNumber == from)
                    .FirstOrDefaultAsync();

                if (member == null)
                {
                    // Creating a new recipient that we haven't seen before

                    // FIXME We're assuming right now that the first message will be their display name

                    member = new Member(from, content);

                    db.Members.Add(member);

                    await db.SaveChangesAsync();
                }

                response.Message($"Thank you for subscribing to {group.DisplayName}!");

                group.Members.Add(new MemberGroup(member.Id, group.Id));

                group.Messages.Add(new Message(group.Id, member.Id, "Joined the group."));
            }
            else
            {
                db.Messages.Add(new Message(group.Id, member.Id, content));
            }

            await db.SaveChangesAsync();

            return TwiML(response);
        }
    }
}