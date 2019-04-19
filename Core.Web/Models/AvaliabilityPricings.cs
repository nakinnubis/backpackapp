using System;
using System.Collections.Generic;

namespace Core.Web.Models
{
    public partial class AvaliabilityPricings
    {
        public int Id { get; set; }
        public int AvaliabilityId { get; set; }
        public int IndividualCategoryId { get; set; }
        public int Price { get; set; }
        public int PriceAfterDiscount { get; set; }

        public Avaliability Avaliability { get; set; }
        public IndividualCategories IndividualCategory { get; set; }
    }
}
