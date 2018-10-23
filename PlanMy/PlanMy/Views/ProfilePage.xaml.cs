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
                string wishlistnbr = await con.DownloadData("https://www.planmy.me/maizonpub-api/wishlist.php?userid=userId&action=getcount", "userid=" + cookie.user.id + "&action=getcount");
                favouriteLabel.Text = wishlistnbr.Replace("\"", "");
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
                Image img = new Image();
                img.Source = item.featured_img;
                img.Margin = new Thickness(10, 0, 0, 0);
                favVendors.Children.Add(img);
            }
            
        }

        private void configBtn_Clicked(object sender, EventArgs e)
        {
            settingsView = new SettingsView(this);
            
            settingsVisible = true;
            settingsView.Margin = new Thickness(0, -Bounds.Height, 0, 0);
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
                Navigation.PushModalAsync(new AddEvent());
            }
            else
            {
                await Navigation.PushModalAsync(new Login());
            }
        }
    }
}