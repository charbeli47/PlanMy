using Newtonsoft.Json;
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
	public partial class newtable : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public newtable(bool isedit, GuestListTables tab)
		{
			InitializeComponent();
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PopModalAsync();
			};
			if (isedit == true && tab != null)

			{
				deletebut.IsVisible = true;
                tablename.Text = tab.Title;
                Pagetitle.Text = "Edit " + tab.Title;
			}

			Savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				if (isedit == true && tab != null)
				{
					edittable(tab);


				}
				else
				{
					addtable();
				}


			};
			deletebut.Clicked += async (object sender, EventArgs e) =>
			{
				deletetable(tab);

			};

		}

		
		

		//function to add task//
		public async void addtable()
		{
            var usr = await GetUser();
            using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("tableName",tablename.Text),
			new KeyValuePair<string, string>("userid",usr.Id.ToString())
				
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/tables.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

            }
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
        //function to edit task//
        public async void edittable(GuestListTables tab)
		{
				
                Connect con = new Connect();
                string json = JsonConvert.SerializeObject(tab);
                con.PostToServer(Statics.apiLink + "GuestListTables/" + tab.Id, json);

				
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
		}

		public void deletetable(GuestListTables tab)
		{

                Connect con = new Connect();
                con.DeleteFromServer(Statics.apiLink + "GuestListTables/" + tab.Id);
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                Navigation.PopModalAsync();
		}
	}

	
}