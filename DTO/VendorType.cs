using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class VendorType
    {   
        public int Id { get; set; }
        public string Title { get; set; }
        public VendorCategory VendorCategory { get; set; }
        public IEnumerable<VendorTypeValue> VendorTypeValues { get; set; }
    }
}
