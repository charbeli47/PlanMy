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
            var fbp = await con.GetData("FaceBookProfile");
            if (!string.IsNullOrEmpty(fbp))
            {
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(fbp);
                ProfileImg.Source = facebookProfile.Picture.Data.Url;
                ProfileImg.WidthRequest = Bounds.Width;
            }
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
        protected async override void OnAppearing()
        {
            Connect con = new Connect();
            var fbp = await con.GetData("FaceBookProfile");
            if (!string.IsNullOrEmpty(fbp))
            {
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(fbp);
                ProfileImg.Source = facebookProfile.Picture.Data.Url;
                ProfileImg.WidthRequest = Bounds.Width;
            }
            base.OnAppearing();
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
    }
}