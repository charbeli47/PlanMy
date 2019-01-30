using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    
    public class UserStats
    {
        public int guestsCount { get; set; }
        public int todosCount { get; set; }
        public int wishesCount { get; set; }
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
        public Users User { get; set; }
    }
}
