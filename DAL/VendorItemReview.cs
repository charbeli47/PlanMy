using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class VendorItemReview
    {
        public long Id { get; set; }
        public string comment { get; set; }
        public string author { get; set; }
        public string email { get; set; }
        public string subject { get; set; }
        public int rating { get; set; }
        public DateTime DateIn { get; set; }
        public VendorItem VendorItem { get; set; }
        public ReviewStatus Status { get; set; }
    }
    public enum ReviewStatus
    {
        Approved,
        Pending,
        Spam
    }
    public enum ReviewRating
    {
        Terrible,
        Poor,
        Average,
        Very_Good,
        Exceptional
    }
}
