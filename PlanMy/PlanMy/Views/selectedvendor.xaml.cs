using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using PlanMy.Views;
using Plugin.Messaging;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class selectedvendor : ContentPage

	{
		ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        protected string catname;
        protected bool isFav;
        protected WordPressPCL.Models.Item selectedpost;

        public selectedvendor(string catname, WordPressPCL.Models.Item selectedpost)
		{
            this.catname = catname;
            this.selectedpost = selectedpost;
			InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadPage();
            
        }
        protected async override void OnAppearing()
        {
            WordpressService service = new WordpressService();
            var media = await service.GetItemMedia(selectedpost.Id);
            imgSlider.ItemsSource = media;
            photoslabel.Text = media.Count().ToString() + " photos";
            base.OnAppearing();
        }
        private async void LoadPage()
        {
            ContentScroll.HeightRequest = Application.Current.MainPage.Height - 100;
            var user = await GetUser();
            if (user.user != null)
            {
                Connect con = new Connect();
                var s = await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=getsingle&userid=" + user.user.id + "&itemid=" + selectedpost.Id);
                if(s == "1")
                {
                    favImg.Source = "favselected.png";
                    isFav = true;
                }                    
                else
                {
                    favImg.Source = "fav.png";
                    isFav = false;
                }
                    
            }
            HtmlWebViewSource load = new HtmlWebViewSource();
            load.Html = "<html style='width:100%;height:100%'><body style='background-image:url(loadingreviews.gif);background-position:center;background-repeat:no-repeat;width:100%;height:100%;background-size:cover' ></body></html>";
            load.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            preload.Source = load;
            dpreload.Source = load;
            Pagetitle.Text = catname;
            backarrow.Clicked += (object sender, EventArgs e) =>
            {
                Navigation.PopModalAsync();
            };
            HtmlWebViewSource html = new HtmlWebViewSource();
            html.Html = selectedpost.Content.Rendered;
            
            Description.Source = html;
            Description.Navigating += (s, e) =>
            {
                if (e.Url.StartsWith("http"))
                {
                    try
                    {
                        var uri = new Uri(e.Url);
                        Xamarin.Forms.Device.OpenUri(uri);
                    }
                    catch (Exception)
                    {
                    }

                    e.Cancel = true;
                }
            };
            reviewsWeb.Source = "https://planmy.me/maizonpub-api/comments.php?itemId=" + selectedpost.Id;
           reviewsWeb.Navigated += ReviewsWeb_Navigated;
            
            Titlepost.Text = selectedpost.Title.Rendered;

            if (selectedpost.ItemMeta.item_address.Length > 0)
                Address.Text = selectedpost.ItemMeta.item_address[0];
            imgSlider.HeightRequest = Application.Current.MainPage.Width * 487 / 1900;
            

            seemap.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async() => {
                    if (selectedpost.ItemMeta.locators.Length > 0)
                    {
                        var perm = await Utils.CheckPermissions(Permission.Location);
                        if (perm == PermissionStatus.Granted)
                        {
                            string locator = selectedpost.ItemMeta.locators[0];
                            var position = await Plugin.Geolocator.CrossGeolocator.Current.GetPositionsForAddressAsync(locator);
                            var latitude = position.FirstOrDefault().Latitude;
                            var longitude = position.FirstOrDefault().Longitude;
                            await Navigation.PushModalAsync(new VendorMap(selectedpost, latitude, longitude));
                        }
                    }
                } ),
            });

            // make phone call///
            phonebut.Clicked += (object sender, EventArgs e) =>
            {
                var PhoneCallTask = CrossMessaging.Current.PhoneDialer;
                string phone = "";
                if (selectedpost.ItemMeta.item_phone.Length > 0)
                    phone = selectedpost.ItemMeta.item_phone[0];
                if (PhoneCallTask.CanMakePhoneCall)
                    PhoneCallTask.MakePhoneCall(phone);
            };
        }
        

        private async void ReviewsWeb_Navigated(object sender, WebNavigatedEventArgs e)
        {
            preload.IsVisible = false;
            string Height = await reviewsWeb.EvaluateJavaScriptAsync("document.height || window.innerHeight || document.body.scrollHeight");
            reviewsWeb.HeightRequest = int.Parse(Height);
            dpreload.IsVisible = false;
            string DHeight = await Description.EvaluateJavaScriptAsync("document.height || window.innerHeight || document.body.scrollHeight");
            Description.HeightRequest = int.Parse(DHeight);
        }

        private async void moreInfoBtn_Clicked(object sender, EventArgs e)
        {
            var user = await GetUser();
            if (user.user != null)
            {
                VendorItem item = new VendorItem();
                if (selectedpost.Embedded.WpFeaturedmedia.Count() > 0)
                    item.featured_media = selectedpost.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl;
                item.post_title = WebUtility.HtmlDecode(selectedpost.Title.Rendered);
                item.post_author = selectedpost.Author.ToString();

                await Navigation.PushModalAsync(new MainChatPage(item));
            }
            else
            {
                await DisplayAlert("Not Logged in", "You need to login to use this feature", "OK");
            }
        }
        public async Task<UserCookie> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = new UserCookie();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            }
            return cookie;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await GetUser();
            if (usr.user != null)
            {
                if (isFav == false)
                {
                    isFav = true;
                    favImg.Source = "favselected.png";
                    await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=insert&userid=" + usr.user.id + "&itemid=" + selectedpost.Id);
                }
                else
                {
                    isFav = false;
                    favImg.Source = "fav.png";
                    await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=delete&userid=" + usr.user.id + "&itemid=" + selectedpost.Id);
                }
            }
            else
            {
                await DisplayAlert("Not Logged in", "You need to login to use this feature", "OK");
            }

        }
    }
	
}