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
                string resp = await con.DownloadData(Statics.apiLink + "ChatChannels", "UserId=" + cookie.Id);
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ChatChannel>>(resp);
                List<Users> users = new List<Users>();
                foreach(var row in result)
                {
                    users.Add(row.Vendor);
                }
                MessagesListView.ItemsSource = users;
            }
        }

        private async void MessagesListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var vendor = (ChatChannel)e.SelectedItem;
            await Navigation.PushModalAsync(new MainChatPage(vendor.VendorId, Statics.MediaLink + vendor.Vendor.Image));

        }
    }
}