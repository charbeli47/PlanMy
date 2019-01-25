using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class CheckList
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public DateTime Timing { get; set; }
        public VendorCategory VendorCategory { get; set; }
        public string Description { get; set; }
        public Users User { get; set; }
        public CheckListStatus Status { get; set; }

    }
    public enum CheckListStatus
    {
        Done,
        ToDo
    }
}
