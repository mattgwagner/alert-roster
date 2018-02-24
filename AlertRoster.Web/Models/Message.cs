using System;
using System.ComponentModel.DataAnnotations;

namespace AlertRoster.Web.Models
{
    public class Message
    {
        [Key]
        public int Id { get; private set; }

        [Required]
        public int SenderId { get; private set; }

        public virtual Member Sender { get; private set; }

        [Required]
        public int GroupId { get; private set; }

        public virtual Group Group { get; private set; }

        [Required] // No Max Length yet?
        public String Content { get; private set; }

        public String DisplayContent => $"{Sender.DisplayName}: {Content}";

        [Required]
        public DateTimeOffset Timestamp { get; private set; }

        public MessageStatus Status { get; private set; }

        public void MarkSent() => Status = MessageStatus.Sent;

        private Message()
        {
            // Parameter-less ctor for EF
        }

        public Message(int groupId, int senderId, String content)
        {
            this.GroupId = groupId;
            this.SenderId = senderId;
            this.Content = content;
            this.Timestamp = DateTimeOffset.UtcNow;
            this.Status = MessageStatus.Unsent;
        }

        public enum MessageStatus : byte
        {
            Unsent = 0,

            Sent = 1
        }
    }
}