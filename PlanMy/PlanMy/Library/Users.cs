using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class Users
    {
        public string id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string age { get; set; }
        public Gender gender { get; set; }
        public string address { get; set; }
        public string city { get; set; }
        public string country { get; set; }
        public UserType userType { get; set; }
        public List<Offers> offers { get; set; }
        public List<VendorItem> vendorItems { get; set; }
        public List<WishList> wishList { get; set; }
        public DateTime creationDate { get; set; } = DateTime.Now;
        public string image { get; set; }
        public string userName { get; set; }
        public string email { get; set; }
        public string phoneNumber { get; set; }
    }
        public enum Gender
    {
        Male,
        Female
    }
    public enum UserType
    {
        Vendor,
        Planner,
        Admin
    }

}
