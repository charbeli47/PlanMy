﻿using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class OffersGallery
    {
        public int Id { get; set; }
        public Offers Offers { get; set; }
        public string Image { get; set; }
        public MediaType MediaType { get; set; }
    }
}
