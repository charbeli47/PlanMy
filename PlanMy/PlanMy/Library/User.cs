using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class UserCookie
    {
        public string status { get; set; }
        public string cookie { get; set; }
        public string cookie_name { get; set; }
        public AppUser user { get; set; }
    }
    public class AppUser
    {
        public int id { get; set; }
        public string username { get; set; }
        public string nicename { get; set; }
        public string email { get; set; }
        public string url { get; set; }
        public string registered { get; set; }
        public string displayname { get; set; }
        public string firstname { get; set; }
        public string lastname { get; set; }
        public string nickname { get; set; }
        public string description { get; set; }
        public Capabilities capabilities { get; set; }
        public string avatar { get; set; }
    }
    public class Capabilities
    {
        public bool customer { get; set; }
    }
    public class RegisterResponse
    {
        public bool success { get; set; }
        public bool loggedin { get; set; }
        public string message { get; set; }
    }
    public class FBRegisterResponse
    {
        public bool success { get; set; }
        public bool loggedin { get; set; }
        public string message { get; set; }
        public UserCookie User { get; set; }
    }
    public class ConfigUser
    {
        public string private_event { get; set; }

        public string first_name { get; set; }
        public string last_name { get; set; }
        public string email { get; set; }
        public string user_weddingdate { get; set; }
        public string user_weddingcity { get; set; }
        public string event_name { get; set; }
        public string event_date { get; set; }
        public string event_location { get; set; }
        public string event_img { get; set; }
    }
}
