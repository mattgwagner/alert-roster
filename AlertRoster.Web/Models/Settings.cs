namespace AlertRoster.Web.Models
{
    public class Settings
    {
        public TwilioSettings Twilio { get; set; }

        public class TwilioSettings
        {
            public string PhoneNumber { get; set; }

            public string AccountSid { get; set; }

            public string AuthToken { get; set; }
        }
    }
}