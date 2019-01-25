using PlanMy.Library;
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
	public partial class SignUp : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public SignUp()
		{
			InitializeComponent();
        }

        private async void SignInLink_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string date = "";
            Connect con = new Connect();
            //string link = "http://weddexonline.com/maizonpub-api/users.php?action=register&username=" + NameEntry.Text + "&email=" + EmailEntry.Text + "&weddingdate=" + date + "&password=" + PasswordEntry.Text;
            var token = await con.GetData("FirebaseToken");
            string link = Statics.apiLink + "Register?Username="+NameEntry.Text+"&Email="+EmailEntry.Text+"&Password="+PasswordEntry.Text+"&Token="+token;
            WebClient client = new WebClient();
            string resp = client.DownloadString(link);
            var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(resp);
            
            await con.SaveData("User", resp);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
            
        }
    }
}