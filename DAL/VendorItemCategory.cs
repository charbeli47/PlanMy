using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class VendorItemCategory
    {
        public int Id { get; set; }
        public VendorCategory VendorCategory { get; set; }
        public VendorItem VendorItem { get; set; }
    }
}
