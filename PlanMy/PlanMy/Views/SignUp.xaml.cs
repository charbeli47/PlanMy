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
            string link = "http://weddexonline.com/maizonpub-api/users.php?action=register&username=" + NameEntry.Text + "&email=" + EmailEntry.Text + "&weddingdate=" + date + "&password=" + PasswordEntry.Text;
            WebClient client = new WebClient();
            string resp = client.DownloadString(link);
            RegisterResponse regResp = Newtonsoft.Json.JsonConvert.DeserializeObject<RegisterResponse>(resp);
            if (regResp.success == true)
            {
                if (regResp.success == true)
                {
                    login();
                    

                }
            }
        }
        private async void login()
        {
            try
            {
                string link = "https://www.weddexonline.com/api/auth/generate_auth_cookie/?username=" + NameEntry.Text + "&password=" + PasswordEntry.Text;
                WebClient client = new WebClient();
                string resp = client.DownloadString(link);
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(resp);
                if (cookie.status == "ok")
                {
                    string usrdetails = client.DownloadString("https://www.planmy.me/maizonpub-api/users.php?action=get&userid=" + cookie.user.id);
                    var u = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(usrdetails);
                    cookie.configUsr = u;
                    var uresp = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                    Connect con = new Connect();
                    await con.SaveData("User", uresp);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await Navigation.PopModalAsync();
                    var token = await con.GetData("FirebaseToken");
                    string updateLink = "https://planmy.me/maizonpub-api/add_device.php?action=updatedevice&userid=" + cookie.user.id + "&token=" + token;
                    client.DownloadString(updateLink);
                }
            }
            catch (Exception ex)
            {
                
            }
        }
    }
}