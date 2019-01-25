using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
     public class VendorItemTypeValue
    {
        public int Id { get; set; }
        public VendorTypeValue VendorTypeValue { get; set; }
        public int VendorTypeValueId { get; set; }
        public VendorItem VendorItem { get; set; }
        public int VendorItemId { get; set; }
    }
}
