using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class VendorItemGallery
    {
        public int Id { get; set; }
        public VendorItem Item { get; set; }
        public string Image { get; set; }
        public MediaType MediaType { get; set; }
    }
}
