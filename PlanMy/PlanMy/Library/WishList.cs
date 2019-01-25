using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class WishList
    {
        public int Id { get; set; }
        public VendorItem VendorItem { get; set; }
        public Users User { get; set; }
    }
}
