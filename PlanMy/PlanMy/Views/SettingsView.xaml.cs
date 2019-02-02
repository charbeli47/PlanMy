using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using Plugin.Share;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SettingsView : ContentView
	{
        ProfilePage _profile;
        public SettingsView(ProfilePage profile)
        {
            _profile = profile;
            LoadPage();
            App.PostSuccessFacebookAction = async token =>
            {
                Connect con = new Connect();
                
                var vm = BindingContext as FacebookViewModel;
                
                var requestUrl =
                "https://graph.facebook.com/v3.1/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages,photos&access_token="
                + token;

                var httpClient = new HttpClient();

                var userJson = await httpClient.GetStringAsync(requestUrl);         
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
                requestUrl = "https://graph.facebook.com/me/picture?type=large&access_token=" + token;
                facebookProfile.Picture.Data.Url = requestUrl;
                await con.SaveData("FaceBookProfile", userJson);
                string password = CreatePassword(8);
                string link = "https://planmy.me/maizonpub-api/users.php?action=fbregister&username=" + facebookProfile.FirstName + "&email=" + facebookProfile.Email + "&password=" + password + "&fb_id=" + facebookProfile.Id;
                WebClient client = new WebClient();
                string resp = client.DownloadString(link);
                FBRegisterResponse regResp = Newtonsoft.Json.JsonConvert.DeserializeObject<FBRegisterResponse>(resp);
                if (regResp.success == true)
                {
                    var uresp = Newtonsoft.Json.JsonConvert.SerializeObject(regResp.User);
                    await con.SaveData("User", uresp);

                    await Navigation.PopModalAsync();
                }
            };
            InitializeComponent();
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
        private async void LoadPage()
        {
            Connect con = new Connect();

            var fbp = await con.GetData("FaceBookProfile");
            if (string.IsNullOrEmpty(fbp))
                logoutBtn.IsVisible = true;
            else
                logoutBtn.IsVisible = false;
            if(_profile.user!=null)
            {
                privateEventSwith.IsToggled = _profile.user.Events.IsPrivate == true;
            }
            else
            {
                helpBtn.IsVisible = false;
            }
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var p = (StackLayout)Parent;
            p.Children.Remove(this);
            _profile.settingsVisible = false;
        }

        private void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            VendorItem vendorItem = new VendorItem { UserId = "98d095b7-f698-4579-bb5b-7d2d936aeb62", Thumb = "chatlogo.png", Title = "Plan My" };
            Navigation.PushModalAsync(new MainChatPage(vendorItem));
        }

        

        private void TapGestureRecognizer_Tapped_3(object sender, EventArgs e)
        {
            CrossShare.Current.Share(new Plugin.Share.Abstractions.ShareMessage
            {
                Text = "Download PlanMy on your device",
                Title = "Share",
                Url="https://www.planmy.me"
            });
        }

        private async void privateEventSwith_Toggled(object sender, ToggledEventArgs e)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            
            if (!string.IsNullOrEmpty(usr))
            {
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                var events = cookie.Events;
                events.IsPrivate = e.Value;
                var json = JsonConvert.SerializeObject(events);
                await con.PostToServer(Statics.apiLink + "Events", json);
            }
            _profile.user.Events.IsPrivate = e.Value;
        }
    }
}