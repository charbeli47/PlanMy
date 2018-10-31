using System;
using System.Collections.Generic;
using System.Text;

namespace PlanMy.ViewModels
{
	public class favoritesobject
	{
		public string icon;
		public string name;
		public string categorie;
	}
    public class VendorItem
    {
        public int ID { get; set; }
        public string post_author { get; set; }
        public string post_date { get; set; }
        public string post_date_gmt { get; set; }
        public string post_content { get; set; }
        public string post_title { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string item_email { get; set; }
        public string post_excerpt { get; set; }
        public string post_status { get; set; }
        public string post_name { get; set; }
        public string guid { get; set; }
        public int menu_order { get; set; }
        public string facebook { get; set; }
        public string googleplus { get; set; }
        public string twitter { get; set; }
        public string youtube { get; set; }
        public string linkedin { get; set; }
        public string pinterest { get; set; }
        public string instagram { get; set; }
        public string featured_media { get; set; }
        public string wide_media { get; set; }
        public string item_address { get; set; }
        public string item_phone { get; set; }
        public bool featured_item { get; set; }
        public bool isfavourite { get; set; }
        public string item_chat { get; set; }
    }
}
