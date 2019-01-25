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
        public ConfigUser user;
        public SettingsView settingsView;
        public ProfilePage()
		{
			InitializeComponent ();
            
            NavigationPage.SetHasNavigationBar(this, false);
            HtmlWebViewSource load = new HtmlWebViewSource();
            load.Html = "<html style='width:100%;height:100%'><body style='background-image:url(loadingreviews.gif);background-position:center;background-repeat:no-repeat;width:100%;height:100%;background-size:cover' ></body></html>";
            load.BaseUrl = DependencyService.Get<IBaseUrl>().Get();
            preload.Source = load;
            LoadFavVendors();
            searchevent.SearchButtonPressed += Searchevent_SearchButtonPressed;
        }

        private void Searchevent_SearchButtonPressed(object sender, EventArgs e)
        {
            var search = searchevent.Text;
            Navigation.PushAsync(new ListEvents(search));
        }
        public async void LoadData()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                user = cookie.configUsr;
                if (!string.IsNullOrEmpty(user.event_img))
                {
                    ProfileImg.Source = user.event_img;
                    StartPlanningBtn.IsVisible = false;
                    planningstarted.IsVisible = true;
                    eventlocation.Text = user.event_location;
                    eventwebsite.Text = user.event_name;
                    DateTime endTime = DateTime.Parse(user.event_date);
                    var timespan = endTime.Subtract(DateTime.Now);
                    Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                    {
                        if (timespan.TotalDays > 0 && timespan.TotalHours > 0 && timespan.TotalMinutes > 0 && timespan.TotalSeconds > 0)
                        {
                            // Do something
                            dayDisplay.Text = timespan.Days.ToString();
                            hourDisplay.Text = timespan.Hours.ToString();
                            minuteDisplay.Text = timespan.Minutes.ToString();
                            secondDisplay.Text = timespan.Seconds.ToString();
                            timespan = timespan.Subtract(new TimeSpan(0, 0, 1));
                            return true;
                        }
                        else
                            return false;

                    });
                }
            }
        }
        public async void LoadFavVendors()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if(!string.IsNullOrEmpty(usr))
            {
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                user = cookie.configUsr;
                if (user == null)
                {
                    string usrdetails = await con.DownloadData("https://www.planmy.me/maizonpub-api/users.php", "action=get&userid=" + cookie.user.id);
                    user = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(usrdetails);
                    cookie.configUsr = user;
                    string resp = Newtonsoft.Json.JsonConvert.SerializeObject(cookie);
                    await con.SaveData("User", resp);
                }
                if (!string.IsNullOrEmpty(user.event_img))
                {
                    ProfileImg.Source = user.event_img;
                    StartPlanningBtn.IsVisible = false;
                    planningstarted.IsVisible = true;
                    eventlocation.Text = user.event_location;
                    eventwebsite.Text = user.event_name;
                    DateTime endTime = DateTime.Parse(user.event_date);
                    var timespan = endTime.Subtract(DateTime.Now);
                    Xamarin.Forms.Device.StartTimer(new TimeSpan(0, 0, 1), () =>
                    {
                        if (timespan.TotalDays > 0 && timespan.TotalHours > 0 && timespan.TotalMinutes > 0 && timespan.TotalSeconds > 0)
                        {
                            // Do something
                            dayDisplay.Text = timespan.Days.ToString();
                            hourDisplay.Text = timespan.Hours.ToString();
                            minuteDisplay.Text = timespan.Minutes.ToString();
                            secondDisplay.Text = timespan.Seconds.ToString();
                            timespan = timespan.Subtract(new TimeSpan(0, 0, 1));
                            return true;
                        }
                        else
                            return false;

                    });
                }
                string stats = await con.DownloadData("https://planmy.me/maizonpub-api/users.php", "action=getstats&userid=" + cookie.user.id);
                var usrStats = Newtonsoft.Json.JsonConvert.DeserializeObject<UserStats>(stats);
                guestsLabel.Text = usrStats.guestsCount.ToString();
                tasksLabel.Text = usrStats.todosCount.ToString();
                favouriteLabel.Text = usrStats.wishesCount.ToString(); ;
                
                
                configBtn.IsVisible = true;
            }
            /*var fbp = await con.GetData("FaceBookProfile");
            if (!string.IsNullOrEmpty(fbp))
            {
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(fbp);
                ProfileImg.Source = facebookProfile.Picture.Data.Url;
                ProfileImg.WidthRequest = Bounds.Width;
            }*/
            /*commit from charbel var featuredItems = await service.GetFeaturedItemsAsync();
            preload.IsVisible = false;
            //WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI(Statics.WooApi, Statics.ConsumerKey, Statics.ConsumerSecret);
            //WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            //var p = wc.Product;
            //var products = await p.GetAll();
            foreach (var item in featuredItems)
            {
                Image img = new Image();
                
                img.Source = item.featured_img;
                img.Margin = new Thickness(10, 0, 0, 0);
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (sender2, args) =>
                {
                    //(MainPage as ContentPage).Content = this.Content;
                    Navigation.PushModalAsync(new selectedvendor(item.Title.Rendered, item),true);
                };
                img.GestureRecognizers.Add(recognizer);
                favVendors.Children.Add(img);
            }*/
            
        }
        

        private void configBtn_Clicked(object sender, EventArgs e)
        {
            settingsView = new SettingsView(this);
            
            settingsVisible = true;
            var bheight = App.Current.MainPage.Height;
            settingsView.Margin = new Thickness(0, -bheight, 0, 0);
            settingsView.HeightRequest = bheight;
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
            LoadData();
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