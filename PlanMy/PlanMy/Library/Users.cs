using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class Users
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Age { get; set; }
        public Gender Gender { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public UserType UserType { get; set; }
        public List<Offers> Offers { get; set; }
        public List<VendorItem> VendorItems { get; set; }
        public List<WishList> WishList { get; set; }
        public List<UserPaymentToken> UserPaymentTokens { get; set; }
        public DateTime CreationDate { get; set; } = DateTime.Now;
        public string Image { get; set; }
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
