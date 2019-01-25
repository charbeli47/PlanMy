using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class OffersGallery
    {
        public int Id { get; set; }
        public Offers Offers { get; set; }
        public string Image { get; set; }
        public MediaType MediaType { get; set; }
    }
    public class OffersGalleryViewModel
    {
        public int Id { get; set; }
        public Offers Offers { get; set; }
        public IFormFile Image { get; set; }
        public MediaType MediaType { get; set; }
    }
}
