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
        protected VendorItem selectedpost;

         public selectedvendor(string catname, VendorItem selectedpost)
         {
             this.catname = catname;
             this.selectedpost = selectedpost;
             InitializeComponent();
             NavigationPage.SetHasNavigationBar(this, false);
             LoadPage();

         }
        protected override void OnAppearing()
        {
            var media = selectedpost.Gallery;
            imgSlider.ItemsSource = media;
            foreach(var r in media)
            {
                r.Image = Statics.MediaLink + r.Image;
            }
            photoslabel.Text = media.Count().ToString() + " photos";
            base.OnAppearing();
        }
        private async void LoadPage()
        {
            ContentScroll.HeightRequest = Application.Current.MainPage.Height - 100;
            var user = await GetUser();
            if (user != null)
            {
                Connect con = new Connect();
                var s = await con.DownloadData(Statics.apiLink + "WishLists/" + selectedpost.Id + "/" + user.Id, "");
                if (s == "1")
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
            html.Html = selectedpost.HtmlDescription;

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
            reviewsWeb.Source = "http://test.planmy.me/AppReviews?VendorItemId=" + selectedpost.Id;
            reviewsWeb.Navigated += ReviewsWeb_Navigated;

            Titlepost.Text = selectedpost.Title;
            
                Address.Text = selectedpost.Address;
            imgSlider.HeightRequest = Application.Current.MainPage.Width * 487 / 1900;


            seemap.GestureRecognizers.Add(new TapGestureRecognizer
            {
                Command = new Command(async () =>
                {
                    var perm = await Utils.CheckPermissions(Permission.Location);
                    if (perm == PermissionStatus.Granted)
                    {
                        var latitude = selectedpost.Latitude != null ? (double)selectedpost.Latitude : 0;
                        var longitude = selectedpost.Longitude != null ? (double)selectedpost.Longitude : 0;
                        await Navigation.PushModalAsync(new VendorMap(selectedpost, latitude, longitude));
                    }
                }),
            });

            // make phone call///
            phonebut.Clicked += (object sender, EventArgs e) =>
            {
                var PhoneCallTask = CrossMessaging.Current.PhoneDialer;
                string phone = "";
                phone = selectedpost.PhoneNumber;
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
            if (user != null)
            {
                VendorItem item = new VendorItem();
                item.Thumb = Statics.MediaLink + selectedpost.Thumb;
                item.Title = WebUtility.HtmlDecode(selectedpost.Title);
                item.UserId = selectedpost.UserId;

                await Navigation.PushModalAsync(new MainChatPage(item.UserId, item.Thumb));
        }
            else
            {
                await DisplayAlert("Not Logged in", "You need to login to use this feature", "OK");
            }
        }
        public async Task<Users> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            }
            return cookie;
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await GetUser();
            if (usr != null)
            {
                if (isFav == false)
                {
                    isFav = true;
                    favImg.Source = "favselected.png";
                    WishList list = new WishList { UserId = usr.Id, VendorItemId = selectedpost.Id };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    await con.PostToServer(Statics.apiLink + "WishLists", json);
                }
                else
                {
                    isFav = false;
                    favImg.Source = "fav.png";
                    con.DeleteFromServer(Statics.apiLink + "WishLists/" + selectedpost.Id + "/" + usr.Id);
                }
            }
            else
            {
                await DisplayAlert("Not Logged in", "You need to login to use this feature", "OK");
            }

        }
    }
	
}