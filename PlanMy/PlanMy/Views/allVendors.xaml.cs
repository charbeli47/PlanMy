using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class allVendors : ContentPage
	{
		
		public IEnumerable<WordPressPCL.Models.Post> specificvendors;
		public string selectedcatname;
		public IEnumerable<WordPressPCL.Models.Post> selectedpost;

		public allVendors (int catid,string catname)
		{
			InitializeComponent ();
			backarrow.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};


			Pagetitle.Text = catname;
			selectedcatname = catname;
			LoadPage(catid);	
		}
		public async void LoadPage(int catid)
		{
			WordpressService service = new WordpressService();
			specificvendors = await service.GetItemsByFilterAsync(catid, null, null, null, null, null, null, null, null, null, null, null, null);
			NumberOfSupplieres.Text = specificvendors.Count().ToString();
			foreach (var post in specificvendors)
			{
				Image img = new Image();
				img.Source = post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl.Replace("https://","http://");
				img.HorizontalOptions = LayoutOptions.FillAndExpand;
				list.Margin = new Thickness(0, 15, 0, 0);
				list.Children.Add(img);

				Frame combo = new Frame();
				combo.BackgroundColor = Color.Red;
				combo.CornerRadius = 10;
				combo.Margin = new Thickness(15, -50, 0, 0);
				combo.HeightRequest = 10;
				combo.WidthRequest = 100;
				Label title = new Label();
				title.Text = post.Title.Rendered;
				title.FontAttributes = FontAttributes.Bold;
				title.Margin = new Thickness(10, 0, 10, 0);
				list.Children.Add(title);
				Label description = new Label();
				description.Margin = new Thickness(10, 0, 10, 0);
				string rendered = WebUtility.HtmlDecode(Regex.Replace(post.Content.Rendered, "<.*?>", ""));
				description.Text = rendered.Length>100?rendered.Substring(0,100)+"more...":rendered;
				list.Children.Add(description);
				Button but = new Button();
				but.Image = "moreinformation.png";
				but.HorizontalOptions =LayoutOptions.End;
				but.Margin = 0;
				but.Padding= 0;
				but.BorderColor = Color.Transparent;
				but.BackgroundColor = Color.Transparent;
				but.Margin = new Thickness(0, 0, 10, 0);
				list.Children.Add(but);
				Headerframe.HeightRequest = 40;
				//selectedpost = (IEnumerable<WordPressPCL.Models.Post>)post;
				but.Clicked += (s, e) => { Navigation.PushModalAsync(new selectedvendor(selectedcatname, post)); };

			}
		}
		

	}



}