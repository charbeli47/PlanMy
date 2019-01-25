using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class GuestList
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public GuestStatus GuestStatus { get; set; }
        public Side Side { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string Country { get; set; }
        public string City { get; set; }
        public GuestListTables GuestListTables { get; set; }
        public Users User { get; set; }

    }
    public enum GuestStatus
    {
        Not_Invited,
        No_Response,
        Accepted,
        Declined
    }
    public enum Side
    {
        Bridesmaids,
        Brides_Family,
        Brides_Friends,
        Brides_Family_Friends,
        Brides_Coworkers,
        Groomsmen,
        Grooms_Family,
        Grooms_Friends,
        Grooms_Family_Friends,
        Grooms_Coworkers,
        Bride_and_Groom_Friends,
        Partner_1,
        Partner_2
    }
}
