using PlanMy.Library;
using PlanMy.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class favoriteView : ContentView
	{
		public favoriteView ()
		{
			InitializeComponent ();
            LoadPage();
            
        }
        async void LoadPage()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                var datas = await con.DownloadData(Statics.apiLink+ "VendorItems/Favorites/"+cookie.Id, "");
                List<VendorItem> vendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(datas);
                
                FavoritesList.FlowItemsSource = vendors;
            }
        }

        private async void FavoritesList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vendor = (VendorItem)e.Item;

            await Navigation.PushModalAsync(new selectedvendor(vendor.Title, vendor));
        }
    }
}