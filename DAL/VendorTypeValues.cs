using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class VendorTypeValue
    {
        public int Id { get; set; }
        public VendorType VendorType { get; set; }
        public int VendorTypeId { get; set; }
        public string Title { get; set; }
        public List<VendorItemTypeValue> VendorItemTypeValues { get; set; }
    }
}
