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
            listmsgs = new List<message>();
            message msg = new message();
            msg.title = "Candid Image";
            msg.msg = "Hello thanks for ....";
            msg.time = "-23h";
            message msg2 = new message();
            msg2.title = "Candid Image";
            msg2.msg = "Hello thanks for ....";
            msg2.time = "-23h";
            listmsgs.Add(msg);
            listmsgs.Add(msg2);
            MessagesListView.ItemsSource = listmsgs;
            
        }
        async void LoadPage()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                var datas = await con.DownloadData("https://planmy.me/maizonpub-api/chat.php", "action=getvendors&my_id=" + cookie.user.id);
                List<VendorItem> vendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(datas);
                foreach (var vendor in vendors)
                    vendor.post_title = WebUtility.HtmlDecode(vendor.post_title);
                MessagesListView.ItemsSource = vendors;
            }
        }

        private void MessagesListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {

        }
    }
}