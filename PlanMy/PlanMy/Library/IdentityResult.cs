using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class IdentityResult
    {
        public bool Succeeded { get; set; }
        public IEnumerable<IdentityError> Errors { get; set; }
    }
    public class IdentityError
    {
        public string Code { get; set; }
        public string Description { get; set; }
    }
}
