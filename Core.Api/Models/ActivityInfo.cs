using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Api.Models
{
    public class ActivityInfo
    {
        public bool Title { get; set; }
        public bool Add_Ons { get; set; }
        public bool Photos { get; set; }
        public bool Locations { get; set; }
        public bool Rules { get; set; }
        public bool IsCompleted { get; set; }

    }
}
