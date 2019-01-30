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
	public partial class messageView : ContentView
	{
        public List<message> listmsgs;
        public messageView ()
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
                var datas = await con.DownloadData("https://planmy.me/maizonpub-api/chat.php", "action=getvendors&my_id=" + cookie.Id);
                List<VendorItem> vendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(datas);
                foreach (var vendor in vendors)
                    vendor.Title = WebUtility.HtmlDecode(vendor.Title);
                MessagesListView.ItemsSource = vendors;
            }
        }

        private async void MessagesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var vendor = (VendorItem)e.SelectedItem;
            await Navigation.PushModalAsync(new MainChatPage(vendor));

        }
    }
}