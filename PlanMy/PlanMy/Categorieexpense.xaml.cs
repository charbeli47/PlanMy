using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Categorieexpense : ContentPage
	{
		public Categorieexpense (expense category)
		{
			InitializeComponent ();
			expendcatname.Text = category.category_name;
			Deleteexpenselabel.Clicked += async (object sender, EventArgs e) =>
			{
				deleteexpensecat(category);
			};

			savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				updateexpensecat(category);
			};

			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};
		}

		public async void deleteexpensecat(expense category)
		{
			
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("category_id",category.category_id)

		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget_category.php?action=delete", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
				Navigation.PushModalAsync(new Planning());

			}
		}
		public async void updateexpensecat(expense category)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("category_id",category.category_id),
				new KeyValuePair<string,string>("category_name",expendcatname.Text)

		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/budget_category.php?action=update", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
				Navigation.PushModalAsync(new Planning());

			}
		}

	}
}