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
        public newtable(bool isedit, table tab)
		{
			InitializeComponent();
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PopModalAsync();
			};
			if (isedit == true && tab != null)

			{
				deletebut.IsVisible = true;
                tablename.Text = tab.tableName;
                Pagetitle.Text = "Edit " + tab.tableName;
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
			new KeyValuePair<string, string>("userid",usr.user.id.ToString())
				
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/tables.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

            }
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
        //function to edit task//
        public async void edittable(table tab)
		{
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("seating_id",tab.seating_id),
			new KeyValuePair<string, string>("tableName",tablename.Text)
				
		});


				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/tables.php?action=update", formcontent);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync();

                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

            }
		}

		public async void deletetable(table tab)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string,string>("seating_id",tab.seating_id)


		});


				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/tables.php?action=delete", formcontent);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync();

                OperationCompleted?.Invoke(this, EventArgs.Empty);
                Navigation.PopModalAsync();

            }
		}
	}

	
}