using NLog;
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
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static String PhoneNumber = ConfigurationManager.AppSettings["Twilio.PhoneNumber"];

        public static String AccountSid = ConfigurationManager.AppSettings["Twilio.AccountSid"];

        public static String AuthToken = ConfigurationManager.AppSettings["Twilio.AuthToken"];

        public static void Send(IEnumerable<String> Recipients, String content)
        {
            var twilio = new TwilioRestClient(AccountSid, AuthToken);

            // TODO Might switch this to running in parallel or batch sending?

            foreach (var recipient in Recipients)
            {
                // TODO Handle errors, notify admin on bad #s

                twilio.SendSmsMessage(PhoneNumber, recipient, content);
            }
        }
    }
}