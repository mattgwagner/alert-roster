using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using Twilio;

namespace alert_roster.web.Models
{
    public class SMSSender
    {
        public static String PhoneNumber = ConfigurationManager.AppSettings["Twilio.PhoneNumber"];

        public static String AccountSid = ConfigurationManager.AppSettings["Twilio.AccountSid"];

        public static String AuthToken = ConfigurationManager.AppSettings["Twilio.AuthToken"];

        public void Send(String content)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            using (var db = new AlertRosterDbContext())
            {
                var recipients = from u in db.Users
                                 where u.SMSEnabled
                                 select u.PhoneNumber;

                foreach (var recipient in recipients)
                {
                    twilio.SendSmsMessage(PhoneNumber, recipient, content);
                }
            }
        }
    }
}