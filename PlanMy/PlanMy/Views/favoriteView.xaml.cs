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
            /*commit from charbel Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                var datas = await con.DownloadData("https://planmy.me/maizonpub-api/wishlist.php", "action=get&userid=" + cookie.user.id);
                List<VendorItem> vendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(datas);
                foreach (var vendor in vendors)
                    vendor.post_title = WebUtility.HtmlDecode(vendor.post_title);
                FavoritesList.FlowItemsSource = vendors;
            }*/
        }

        private async void FavoritesList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var vendor = (VendorItem)e.Item;
            /*commit from charbel WordpressService service = new WordpressService();
            var post = await service.GetItemByIDAsync(vendor.ID);

            await Navigation.PushModalAsync(new selectedvendor(vendor.post_title, post));*/
        }
    }
}