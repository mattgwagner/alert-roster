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
        public static String FromAddress = ConfigurationManager.AppSettings["Email.FromAddress"];

        public static String SmtpServer = ConfigurationManager.AppSettings["MAILGUN_SMTP_SERVER"];

        public static int SmtpPort = int.Parse(ConfigurationManager.AppSettings["MAILGUN_SMTP_PORT"]);

        public static String SmtpUser = ConfigurationManager.AppSettings["MAILGUN_SMTP_LOGIN"];

        public static String SmtpPassword = ConfigurationManager.AppSettings["MAILGUN_SMTP_PASSWORD"];

        public static Boolean EnableSsl = true;

        public static Boolean IsBodyHtml = false;

        public void Send(String content, IEnumerable<String> Recipients)
        {
            using (var smtp = new SmtpClient { Host = SmtpServer, Port = SmtpPort, EnableSsl = EnableSsl, Credentials = new NetworkCredential { UserName = SmtpUser, Password = SmtpPassword } })
            using (var message = new MailMessage { IsBodyHtml = IsBodyHtml })
            {
                message.From = new MailAddress("");

                message.To.Add(new MailAddress(""));

                foreach (var recipient in Recipients)
                {
                    message.Bcc.Add(new MailAddress(recipient));
                }

                message.Subject = "";

                message.Body = "";

                smtp.Send(message);
            }
        }
    }
}