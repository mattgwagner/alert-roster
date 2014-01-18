using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Web;

namespace alert_roster.web.Models
{
    public class EmailSender
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public static String EmailSubject = ConfigurationManager.AppSettings["Email.Subject"];

        public static String FromAddress = ConfigurationManager.AppSettings["Email.FromAddress"];

        public static String SmtpServer = ConfigurationManager.AppSettings["MAILGUN_SMTP_SERVER"];

        public static int SmtpPort = int.Parse(ConfigurationManager.AppSettings["MAILGUN_SMTP_PORT"]);

        public static String SmtpUser = ConfigurationManager.AppSettings["MAILGUN_SMTP_LOGIN"];

        public static String SmtpPassword = ConfigurationManager.AppSettings["MAILGUN_SMTP_PASSWORD"];

        public static Boolean EnableSsl = true;

        public static Boolean IsBodyHtml = false;

        public static void Send(String content)
        {
            using (var db = new AlertRosterDbContext())
            using (var smtp = new SmtpClient { Host = SmtpServer, Port = SmtpPort, EnableSsl = EnableSsl, Credentials = new NetworkCredential { UserName = SmtpUser, Password = SmtpPassword } })
            using (var message = new MailMessage { IsBodyHtml = IsBodyHtml })
            {
                var recipients = (from u in db.Users where u.EmailEnabled select u.EmailAddress);

                if (recipients.Any())
                {
                    message.From = new MailAddress(FromAddress);

                    message.Subject = EmailSubject;

                    foreach (var recipient in recipients)
                    {
                        message.Bcc.Add(new MailAddress(recipient));
                    }

                    message.Body = content;

                    smtp.Send(message);
                }
            }
        }
    }
}