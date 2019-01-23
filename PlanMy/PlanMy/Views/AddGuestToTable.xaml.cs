using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class AddGuestToTable : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public AddGuestToTable(table tab)
		{
			InitializeComponent();
            
            LoadPage(tab);
            
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PopModalAsync();
			};
			

			Savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				if(guestlist.SelectedItem == null)
                {
                    await DisplayAlert("ALERT", "Please choose a guest in order to save", "OK");
                }
                else
                {
                    var guest = (guest)guestlist.SelectedItem;
                    Connect con = new Connect();
                    string resp = await con.DownloadData("https://planmy.me/maizonpub-api/guestlist.php", "action=addtotable&list_id=" + guest.list_id + "&seating_id=" + tab.seating_id);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await DisplayAlert("SUCCESS", "Guest added to table " + tab.tableName, "OK");
                    await Navigation.PopModalAsync();
                }
			};

		}
        public async Task<UserCookie> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = new UserCookie();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            }
            return cookie;
        }
        private async void LoadPage(table tab)
        {
            Connect con = new Connect();
            var usr = await GetUser();
            var data = await con.DownloadData("https://planmy.me/maizonpub-api/guestlist.php", "action=getaccepted&userid=" + usr.user.id);
            var guests = Newtonsoft.Json.JsonConvert.DeserializeObject<List<guest>>(data);
            guestlist.ItemsSource = guests;
        }

        
	}

	
}