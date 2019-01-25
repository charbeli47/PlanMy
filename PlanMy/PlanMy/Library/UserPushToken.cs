using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class UserPushToken
    {
        public long Id { get; set; }
        public string UserId { get; set; }
        public Users User { get; set; }
        public string Token { get; set; }
        public PushDevice PushDevice { get; set; }
    }
    public enum PushDevice
    {
        Android,
        iOS,
        Web
    }
}
