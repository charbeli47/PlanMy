using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdeasPage : ContentPage
	{

		public List<Blog> latestpostsblog;
		public ObservableCollection<Models.blogpost> listposts;
		//public Models.blogpost lasttappeditem;
		//public System.Windows.Input.ICommand TappedCommand { get; set; }
		public IdeasPage ()
		{
            InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);


            //TappedCommand = new Command(() => gotosingleidea());
            //BindingContext = this;
            WebClient client = new WebClient();
            string resp = client.DownloadString(Statics.apiLink + "Blogs");
            latestpostsblog = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Blog>>(resp);
            listposts = new ObservableCollection<Models.blogpost>();
            foreach (var itemData in latestpostsblog)
            {
                Models.blogpost i = new Models.blogpost();
                string post = Newtonsoft.Json.JsonConvert.SerializeObject(itemData);
                string html = Library.HtmlTools.WrapContent(itemData);
                var imgurl = Statics.MediaLink + itemData.Image;
                //go to another page with WebView and display the html(Photos 36)
                i.post = post;
                i.html = html;
                i.imgurl = imgurl.ToString();
                i.title = WebUtility.HtmlDecode(itemData.Title);
                //i.description = itemData.Excerpt.Rendered;
                listposts.Add(i);
            }
            BlogList.ItemsSource = listposts;
            
			//if (lasttappeditem != null)
			//{
				//Navigation.PushModalAsync(new selectedidea(lasttappeditem.html));

			//}
			BlogList.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{

				if (e.SelectedItem == null)
				{
					return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
				}

				var selectedpost = e.SelectedItem as Models.blogpost;

				if (selectedpost == null)
					return;
				((ListView)sender).SelectedItem = null;
				//var newpage = new allVendors(selectedvendor.Id,selectedvendor.Name);
				Navigation.PushModalAsync(new selectedidea(selectedpost.html));
			};
        }

        //public void gotosingleidea()
        //{
        /// okkay hon baddde rouh 3al next page////
        /// 
        //Navigation.PushModalAsync(new selectedidea(lasttappeditem.html));

        ///	}
    }

	

}