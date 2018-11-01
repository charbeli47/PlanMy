using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
		public Login ()
		{
			InitializeComponent ();
            App.PostSuccessFacebookAction = async token =>
            {
                var vm = BindingContext as FacebookViewModel;

                var requestUrl =
                "https://graph.facebook.com/v3.1/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages,photos&access_token="
                + token;

                var httpClient = new HttpClient();

                var userJson = await httpClient.GetStringAsync(requestUrl);
                Connect con = new Connect();                
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
                requestUrl = "https://graph.facebook.com/me/picture?type=large&access_token=" + token;
                facebookProfile.Picture.Data.Url = requestUrl;
                await con.SaveData("FaceBookProfile", userJson);
                string password = CreatePassword(8);
                string link = "https://planmy.me/maizonpub-api/users.php?action=fbregister&username=" + facebookProfile.FirstName + "&email=" + facebookProfile.Email + "&password=" + password+"&fb_id=" + facebookProfile.Id;
                WebClient client = new WebClient();
                string resp = client.DownloadString(link);
                 FBRegisterResponse regResp = Newtonsoft.Json.JsonConvert.DeserializeObject<FBRegisterResponse>(resp);
                if (regResp.success == true)
                {
                    string usrdetails = await con.DownloadData("https://www.planmy.me/maizonpub-api/users.php", "action=get&userid=" + regResp.User.user.id);
                    var u = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(usrdetails);
                    regResp.User.configUsr = u;
                    var uresp = Newtonsoft.Json.JsonConvert.SerializeObject(regResp.User);
                    await con.SaveData("User", uresp);
                    await Navigation.PopModalAsync();
                }
                
                //you can use this token to authenticate to the server here
                //call your FacebookLoginService.LoginToServer(token)
                //I'll just navigate to a screen that displays the token:
                //await Navigation.PushAsync(new DiplayTokenPage(token));
                

            };
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private async void SignUpLink_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new SignUp());
        }

        private async void SkipBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string link = "https://planmy.me/api/auth/generate_auth_cookie/?username=" + UsernameEntry.Text + "&password=" + PasswordEntry.Text;
            WebClient client = new WebClient();
            string resp = client.DownloadString(link);
            UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(resp);
            if (cookie.status == "ok")
            {
                Connect con = new Connect();
                string usrdetails = await con.DownloadData("https://www.planmy.me/maizonpub-api/users.php", "action=get&userid=" + cookie.user.id);
                var u = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(usrdetails);
                cookie.configUsr = u;
                var uresp = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                await con.SaveData("User", uresp);
                con.DeleteData("FaceBookProfile");
                await Navigation.PopModalAsync();
            }
            else
            {
            }
        }

        private async void fbBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FacebookProfilePage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPassword());
        }
    }
}