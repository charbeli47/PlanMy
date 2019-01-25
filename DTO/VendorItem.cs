using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DTO
{
    public class VendorItem
    {
        public int Id { get; set; }
        public  string Title { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string Location { get; set; }
        public string PhoneNumber { get; set; }
        public Users User { get; set; }
        public string HtmlDescription { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public bool IsFeatured { get; set; }
        public string Thumb { get; set; }
        public Country Country { get; set; }
        public List<VendorItemGallery> Gallery { get; set; }
        public List<VendorItemCategory> Categories { get; set; }
        public List<VendorItemTypeValue> VendorItemTypeValues { get; set; }
        public MediaType MediaType { get; set; }
        public List<VendorItemReview> VendorItemReviews { get; set; }
    }
    
}
