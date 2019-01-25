using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.Library
{
     public class BlogCategory
    {
      
        public int Id { get; set; }
        public string Image { get; set; }
        public string Title { get; set; }
        public string Media { get; set; }
        public List<BlogCategoryRelation> Categories { get; set; }
        public MediaType MediaType { get; set; }
    }
}
