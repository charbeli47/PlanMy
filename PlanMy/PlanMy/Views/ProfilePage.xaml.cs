using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        public bool settingsVisible = false;
        protected ConfigUser user;
        public SettingsView settingsView;
        public ProfilePage()
		{
			InitializeComponent ();
            
            NavigationPage.SetHasNavigationBar(this, false);
            LoadFavVendors();
        }
        async void LoadFavVendors()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if(!string.IsNullOrEmpty(usr))
            {
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                string guestsnbr = await con.DownloadData("https://www.planmy.me/maizonpub-api/guestlist.php", "userid=" + cookie.user.id + "&action=getcount");
                guestsLabel.Text = guestsnbr.Replace("\"","");
                string todorecord = await con.DownloadData("https://www.planmy.me/maizonpub-api/todolist.php", "todo_user=" + cookie.user.id + "&action=getcount");
                tasksLabel.Text = todorecord.Replace("\"", "");
                string wishlistnbr = await con.DownloadData("https://www.planmy.me/maizonpub-api/wishlist.php", "userid=" + cookie.user.id + "&action=getcount");
                favouriteLabel.Text = wishlistnbr.Replace("\"", "");
                string usrdetails = await con.DownloadData("https://www.planmy.me/maizonpub-api/users.php", "action=get&userid=" + cookie.user.id);
                user = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(usrdetails);
                if (!string.IsNullOrEmpty(user.event_img))
                {
                    ProfileImg.Source = user.event_img;
                    StartPlanningBtn.IsVisible = false;
                    planningstarted.IsVisible = true;
                }
            }
            /*var fbp = await con.GetData("FaceBookProfile");
            if (!string.IsNullOrEmpty(fbp))
            {
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(fbp);
                ProfileImg.Source = facebookProfile.Picture.Data.Url;
                ProfileImg.WidthRequest = Bounds.Width;
            }*/
            WordpressService service = new WordpressService();
            var featuredItems = await service.GetFeaturedItemsAsync();
            //WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI(Statics.WooApi, Statics.ConsumerKey, Statics.ConsumerSecret);
            //WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            //var p = wc.Product;
            //var products = await p.GetAll();
            foreach (var item in featuredItems)
            {
                Button img = new Button();
                img.Image = item.featured_img;
                img.Margin = new Thickness(10, 0, 0, 0);
                img.Clicked += (s, e) => {
                    //Navigation.PushAsync(new selectedvendor(item.Title.Rendered,item));
                };
                favVendors.Children.Add(img);
            }
            
        }

        private void configBtn_Clicked(object sender, EventArgs e)
        {
            settingsView = new SettingsView(this);
            
            settingsVisible = true;
            settingsView.Margin = new Thickness(0, -Bounds.Height - 10, 0, 0);
            settingsView.HeightRequest = Bounds.Height;
            ContentStack.Children.Add(settingsView);

        }
        protected override bool OnBackButtonPressed()
        {
            if (settingsVisible)
            {
                ContentStack.Children.Remove(settingsView);
                settingsVisible = false;
                return true;
            }
            else
                return false;
        }
        private async void eventEditBtn_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                var eventPage = new AddEvent(user);
                eventPage.OperationCompleted += EventPage_OperationCompleted;
                await Navigation.PushModalAsync(eventPage);
            }
            else
            {
                await Navigation.PushModalAsync(new Login());
            }
        }

        private void EventPage_OperationCompleted(object sender, EventArgs e)
        {
            LoadFavVendors();
        }

        private async void StartPlanningBtn_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                var eventPage = new AddEvent(user);
                eventPage.OperationCompleted += EventPage_OperationCompleted;
                await Navigation.PushModalAsync(eventPage);
            }
            else
            {
                await Navigation.PushModalAsync(new Login());
            }
        }
    }
}