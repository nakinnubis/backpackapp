using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class User_Favorite_Activities
    {
        public int id { get; set; }
        public int activity_id { get; set; }
        public int user_id { get; set; }
        public Nullable<DateTime>  additiondate { get; set; }
    }
}
