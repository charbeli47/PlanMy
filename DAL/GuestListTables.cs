using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class GuestListTables
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Users User { get; set; }
    }
}
