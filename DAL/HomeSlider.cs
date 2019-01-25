using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class HomeSlider
    {
        public int Id { get; set; }
        public string Media { get; set; }
        public MediaType MediaType { get;set; }
    }
    public class HomeSliderViewModel
    {
        public int Id { get; set; }
        public IFormFile Media { get; set; }
        public MediaType MediaType { get; set; }
    }

    public enum MediaType
    {
        Video,
        Image
    }
    
}
