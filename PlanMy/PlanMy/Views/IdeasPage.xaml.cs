using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class IdeasPage : ContentPage
	{

		public IEnumerable<WordPressPCL.Models.Post> latestpostsblog;
		public ObservableCollection<Models.blogpost> listposts;
		//public Models.blogpost lasttappeditem;
		//public System.Windows.Input.ICommand TappedCommand { get; set; }
		public IdeasPage ()
		{
			InitializeComponent ();
	
		//TappedCommand = new Command(() => gotosingleidea());
			//BindingContext = this;
			WordpressService service = new WordpressService();
			Task.Run(async () =>
			{
				latestpostsblog = await service.GetLatestPostsAsync();
				listposts = new ObservableCollection<Models.blogpost>();
				foreach (var itemData in latestpostsblog)
				{
					Models.blogpost i = new Models.blogpost();
					string post = Newtonsoft.Json.JsonConvert.SerializeObject(itemData);
					string html = Library.HtmlTools.WrapContent(itemData);
					var imgurl = itemData.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl;
					//go to another page with WebView and display the html(Photos 36)
					i.post = post;
					i.html = html;
					i.imgurl = imgurl.ToString();
					listposts.Add(i);
				}


				BlogList.FlowItemsSource = listposts;
			});


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