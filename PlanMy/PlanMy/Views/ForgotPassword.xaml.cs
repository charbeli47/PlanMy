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
	public partial class ForgotPassword : ContentPage
	{
		public ForgotPassword()
		{
			InitializeComponent();
		}

		private void Button_Clicked(object sender, EventArgs e)

		{

			getresponsefromserver();

		}

		public async void getresponsefromserver()
		{
			try
			{
				Connect con = new Connect();
				MultipartFormDataContent data = new MultipartFormDataContent();
				var username = new StringContent(UsernameEntry.Text.ToString());


				data.Add(username, "Username");

				string resp = await con.PostToServer(Statics.apiLink + "ForgotPassword", data);
				servermsg.Text = resp;
				
			}
			catch
			{

			}
		}
	}
}