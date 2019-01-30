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
        public AddGuestToTable(GuestListTables tab)
		{
			InitializeComponent();
            
            LoadPage();
            
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
                    var guest = (GuestList)guestlist.SelectedItem;
                    Connect con = new Connect();
                    string link = Statics.apiLink + "GuestLists/" + guest.Id;
                    guest.GuestListTables = tab;
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(guest);
                    con.PostToServer(link, json);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await DisplayAlert("SUCCESS", "Guest added to table " + tab.Title, "OK");
                    await Navigation.PopModalAsync();
                }
			};

		}
        public async Task<Users> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            }
            return cookie;
        }
        private async void LoadPage()
        {
            Connect con = new Connect();
            var usr = await GetUser();
            var data = await con.DownloadData(Statics.apiLink+"GuestLists", "UserId=" + usr.Id);
            var guests = Newtonsoft.Json.JsonConvert.DeserializeObject<List<GuestList>>(data);
            guestlist.ItemsSource = guests;
        }

        
	}

	
}