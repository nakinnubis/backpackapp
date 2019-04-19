using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Chat
    {
        [Key]
        public int chatId { get; set; }
        public DateTime lastUpdateDate { get; set; }

        [ForeignKey("user")]
        public int firstUser { get; set; }
        [ForeignKey("user1")]
        public int secondUser { get; set; }


        public virtual User user { get; set; }
        public virtual User user1 { get; set; }
        public virtual ICollection<MessageReply> MessageReplies { get; set; }
    }
}
