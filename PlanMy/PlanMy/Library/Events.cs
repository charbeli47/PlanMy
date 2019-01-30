
using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class Events
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime Date { get; set; }
        public string Image { get; set; }
        public string UserId { get; set; }
        public bool IsPrivate { get; set; }
    }
}
