using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class MessageReply
    {
        [Key]
        public int messageReplayId { get; set; }
        [ForeignKey("Chat")]
        public int chatId { get; set; }
        [ForeignKey("user")]
        public int userId { get; set; }
        public int regardingId { get; set; }
        public int typeId { get; set; }
        public string message { get; set; }
        public DateTime date { get; set; }
        public bool isReadable { get; set; }
        public virtual User user { get; set; }
        public virtual Chat Chat { get; set; }
    }
}
