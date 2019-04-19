using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Activity_Photos
    {

        public int id { get; set; }

        [ForeignKey("Activity")]
        public Nullable<int> activity_id { get; set; }
        public string url { get; set; }
        public bool cover_photo { get; set; }

        public virtual Activity Activity { get; set; }
    }
}
