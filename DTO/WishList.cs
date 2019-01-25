using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class WishList
    {
        public int Id { get; set; }
        public VendorItem VendorItem { get; set; }
        public Users User { get; set; }
    }
}
