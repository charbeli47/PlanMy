﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
    public class SocialMedia
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Link { get; set; }
        public string Image { get; set; }
        public MediaType MediaType { get; set; }
    }
}
