using Core.Web.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Web.Models
{
    public class Avaliability_Pricing
    {
        [Key]
        public int id { get; set; }

        [ForeignKey("Avaliability")]
        public int avaliabilityId { get; set; }

        [ForeignKey("IndividualCategory")]
        public int individualCategoryId { get; set; }
        public int price { get; set; }
        public int priceAfterDiscount { get; set; }

        public virtual Avaliability Avaliability { get; set; }
        public virtual IndividualCategory IndividualCategory { get; set; }

    }

}
