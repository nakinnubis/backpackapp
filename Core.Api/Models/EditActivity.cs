using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class ActivityOption
    {
        public string option_id { get; set; }
        public string fromAge { get; set; }
        public string toAge { get; set; }
        public string name { get; set; }
    }

    public class EditActivity
    {
        public string type_id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public List<ActivityOption> Activity_Options { get; set; }
    }
    //public class EditActivity
    //{
    //    public string ActivityName { get; set; }

    //    public string Title { get; set; }

    //    public int OptionId { get; set; }

    //    public string Description { get; set; }

    //    public int FromAge { get; set; }

    //    public int ToAge { get; set; }

    //    public int ActivityOptionId { get; set; }

    //    public int TypeId { get; set; }
    //}
}
