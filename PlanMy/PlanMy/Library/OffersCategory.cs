using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class OffersCategory
    {
        public int Id { get; set; }
        public Offers Offers { get; set; }
        public VendorCategory VendorCategory { get; set; }
    }
}
