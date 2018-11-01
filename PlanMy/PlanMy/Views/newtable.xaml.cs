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
		public newtable(bool isedit, table tab)
		{
			InitializeComponent();
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};
			if (isedit == true && tab != null)

			{
				deletebut.IsVisible = true;
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
			
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("tableName",tablename.Text),
			new KeyValuePair<string, string>("userid","169")
				
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/tables.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
				Navigation.PushModalAsync(new Planning());

			}
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

				Navigation.PushModalAsync(new Planning());

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

				Navigation.PushModalAsync(new Planning());

			}
		}
	}

	
}