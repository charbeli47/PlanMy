using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DAL
{
    public class HomeTips
    {
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Media { get; set; }
        public MediaType MediaType { get; set; }
    }
    public class homeTipsViewModel
    {
        
        public int Id { get; set; }
        public IFormFile Image { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public MediaType MediaType { get; set; }

    }
}
