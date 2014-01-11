using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace alert_roster.web.Models
{
    public class Message
    {
        public int ID { get; set; }

        public DateTime PostedDate { get; set; }

        [Required]
        public String Content { get; set; }
    }

    public static class DateTimeExtensions
    {
        private static readonly DateTime Epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        public static double GetEpochTicks(this DateTime dateTime)
        {
            return dateTime.Subtract(Epoch).TotalMilliseconds;
        }
    }
}