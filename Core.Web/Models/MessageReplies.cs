using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class MessageReplies
    {
        public int MessageReplayId { get; set; }
        public int ChatId { get; set; }
        public int UserId { get; set; }
        public int RegardingId { get; set; }
        public int TypeId { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; }
        public bool IsReadable { get; set; }

        public Chat Chat { get; set; }
        public User User { get; set; }
    }
}
