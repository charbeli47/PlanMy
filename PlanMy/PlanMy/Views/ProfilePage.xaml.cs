using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using System;
using System.Collections.Generic;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class ProfilePage : ContentPage
	{
        public bool settingsVisible = false;
        public Users user;
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
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                user = cookie;
                if (!string.IsNullOrEmpty(user.Events.Image))
                {
                    ProfileImg.Source = user.Events.Image;
                    StartPlanningBtn.IsVisible = false;
                    planningstarted.IsVisible = true;
                    eventlocation.Text = user.Events.Description;
                    eventwebsite.Text = user.Events.Title;
                    DateTime endTime = user.Events.Date;
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
                configBtn.IsVisible = true;
            }
        }
        public async void LoadFavVendors()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if(!string.IsNullOrEmpty(usr))
            {
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                user = cookie;
                
                if (!string.IsNullOrEmpty(user.Events.Image))
                {
                    ProfileImg.Source = user.Events.Image;
                    StartPlanningBtn.IsVisible = false;
                    planningstarted.IsVisible = true;
                    eventlocation.Text = user.Events.Description;
                    eventwebsite.Text = user.Events.Title;
                    DateTime endTime = user.Events.Date;
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
                string stats = await con.DownloadData(Statics.apiLink+ "UserStats", "UserId=" + cookie.Id);
                var usrStats = Newtonsoft.Json.JsonConvert.DeserializeObject<UserStats>(stats);
                guestsLabel.Text = usrStats.guestsCount.ToString();
                tasksLabel.Text = usrStats.todosCount.ToString();
                favouriteLabel.Text = usrStats.wishesCount.ToString();
                
                
                configBtn.IsVisible = true;
            }
            /*var fbp = await con.GetData("FaceBookProfile");
            if (!string.IsNullOrEmpty(fbp))
            {
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(fbp);
                ProfileImg.Source = facebookProfile.Picture.Data.Url;
                ProfileImg.WidthRequest = Bounds.Width;
            }*/
            string fresp = await con.DownloadData(Statics.apiLink + "VendorItems/Featured", "");
            var featuredItems = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(fresp);
            preload.IsVisible = false;
            //WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI(Statics.WooApi, Statics.ConsumerKey, Statics.ConsumerSecret);
            //WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            //var p = wc.Product;
            //var products = await p.GetAll();
            foreach (var item in featuredItems)
            {
                Image img = new Image();
                
                img.Source = Statics.MediaLink + item.Thumb;
                img.Margin = new Thickness(10, 0, 0, 0);
            TapGestureRecognizer recognizer = new TapGestureRecognizer();
                recognizer.Tapped += (sender2, args) =>
                {
                    //(MainPage as ContentPage).Content = this.Content;
                    Navigation.PushModalAsync(new selectedvendor(item.Title, item),true);
                };
                img.GestureRecognizers.Add(recognizer);
                favVendors.Children.Add(img);
            }
            
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
                user = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
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