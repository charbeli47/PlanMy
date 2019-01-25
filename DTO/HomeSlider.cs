using System;
using System.Collections.Generic;
using System.Text;

namespace DTO
{
    public class HomeSlider
    {
        public int Id { get; set; }
        public string Media { get; set; }
        public MediaType MediaType { get;set; }
    }

    public enum MediaType
    {
        Video,
        Image
    }
    
}
