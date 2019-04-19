using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class FollowUpHealth
    {
        [Key]
        public int FollowUpHealthId { get; set; }
        [ForeignKey("user")]
        public int user_id { get; set; }
        public bool hospitalization { get; set; }
        public string hospitalizationDetails { get; set; }
        public bool dietaryRestrictions { get; set; }
        public string dietaryRestrictionsDetails { get; set; }
        public bool medication { get; set; }
        public string medicationDetails { get; set; }

        public virtual User user { get; set; }
    }
}
