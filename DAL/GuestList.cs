using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DAL
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
        [Display(Name = "Not Invited")]
        Not_Invited,
        [Display(Name = "No Response")]
        No_Response,
        Accepted,
        Declined
    }
    public enum Side
    {
        Bridesmaids,
        [Display(Name = "Bride's Family")]
        Brides_Family,
        [Display(Name = "Bride's Friends")]
        Brides_Friends,
        [Display(Name = "Bride's Family Friends")]
        Brides_Family_Friends,
        [Display(Name = "Bride's Coworkers")]
        Brides_Coworkers,
        Groomsmen,
        [Display(Name = "Groom's Family")]
        Grooms_Family,
        [Display(Name = "Groom's Friends")]
        Grooms_Friends,
        [Display(Name = "Groom's Family Friends")]
        Grooms_Family_Friends,
        [Display(Name = "Groom's Coworkers")]
        Grooms_Coworkers,
        [Display(Name = "Bride and Groom Friends")]
        Bride_and_Groom_Friends,
        [Display(Name = "Partner 1")]
        Partner_1,
        [Display(Name = "Prtner 2")]
        Partner_2
    }
}
