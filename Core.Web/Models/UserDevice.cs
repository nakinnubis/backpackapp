using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class UserDevice
    {
      
            [Key]
            public int Id { get; set; }
            [ForeignKey("User")]
            public int  userId { get; set; } 
            public virtual bool IsPushEnabled { get; set; }
            public virtual string PushKey { get; set; }
            public virtual User User { get; set; }
       
    }
}
