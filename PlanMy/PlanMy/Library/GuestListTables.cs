using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class GuestListTables
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Users User { get; set; }
        public string UserId { get; set; }
    }
}
