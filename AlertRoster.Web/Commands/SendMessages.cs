using AlertRoster.Web.Models;
using FluentScheduler;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using Twilio.Clients;
using Twilio.Rest.Api.V2010.Account;

namespace AlertRoster.Web.Commands
{
    public class JobRegistry : Registry
    {
        public JobRegistry(IServiceProvider provider)
        {
            var factory = provider.GetRequiredService<IServiceScopeFactory>();

            using (var scope = factory.CreateScope())
            {
                Schedule(() => new SendMessages(scope.ServiceProvider.GetService<Database>(), scope.ServiceProvider.GetService<IOptions<Settings>>()))
                    .NonReentrant()
                    .ToRunEvery(interval: 3).Seconds()
                    .DelayFor(30).Seconds();
            }
        }
    }

    public class SendMessages : IJob
    {
        private readonly Database db;

        public SendMessages(Database db, IOptions<Settings> settings)
        {
            this.db = db;

            Twilio.TwilioClient.Init(settings.Value.Twilio.AccountSid, settings.Value.Twilio.AuthToken);
        }

        public virtual void Execute()
        {
            var client = Get_Client();

            foreach (var message in Get_Unsent())
            {
                foreach (var recipient in Get_Recipients(message))
                {
                    // Don't send the sender their own message back

                    // HACK: For testing also echo it back

                    if (true || message.SenderId != recipient.Id)
                    {
                        MessageResource.Create(
                            to: new Twilio.Types.PhoneNumber(recipient.PhoneNumber),
                            from: new Twilio.Types.PhoneNumber(message.Group.PhoneNumber),
                            body: message.DisplayContent
                        );
                    }
                }

                message.MarkSent();
            }

            db.SaveChanges();
        }

        private IEnumerable<Member> Get_Recipients(Message message)
        {
            switch (message.Group.Replies)
            {
                case Group.ReplyMode.ReplyAll:
                    return message.Group.Members.Select(_ => _.Member);

                case Group.ReplyMode.ReplyToAdmins:
                    return message.Group.Admins;

                case Group.ReplyMode.Reject:
                default:
                    return Enumerable.Empty<Member>();
            }
        }

        private ITwilioRestClient Get_Client()
        {
            return Twilio.TwilioClient.GetRestClient();
        }

        private IEnumerable<Message> Get_Unsent()
        {
            return
                db
                .Messages
                .Include(message => message.Group)
                .ThenInclude(group => group.Members)
                .Include(message => message.Sender)
                .Where(_ => _.Status == Message.MessageStatus.Unsent)
                .ToList();
        }
    }
}