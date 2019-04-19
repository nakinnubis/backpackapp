using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class ActivityPhotos
    {
        public int Id { get; set; }
        public int? ActivityId { get; set; }
        public string Url { get; set; }
        public bool CoverPhoto { get; set; }

        public Activity Activity { get; set; }
    }
}
