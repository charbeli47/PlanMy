using PlanMy.Library;
using PlanMy.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class allVendors : ContentPage, INotifyPropertyChanged
    {
		
		public IEnumerable<WordPressPCL.Models.Item> specificvendors;
		public string selectedcatname;
		public IEnumerable<WordPressPCL.Models.Item> selectedpost;
        protected int _catid;
        protected int page;
        protected bool again = true;
        public List<int> type = new List<int>(), city = new List<int>(), setting = new List<int>(), cateringservicesInt = new List<int>(), typeoffurnitureInt = new List<int>(), clienteleInt = new List<int>(), clothingInt = new List<int>(), beautyservicesInt = new List<int>(), typeofmusiciansInt = new List<int>(), itemlocationInt = new List<int>(), typeofserviceInt = new List<int>(), capacityInt = new List<int>(), honeymoonexperienceInt = new List<int>();

        private void list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            BindingItem post = e.SelectedItem as BindingItem;
            ((ListView)sender).SelectedItem = null;
            if (post == null)
                return;
            Navigation.PushModalAsync(new selectedvendor(post.Title, post.item));
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            Connect con = new Connect();
            var usr = await GetUser();
            if (usr.user != null)
            {
                Image img = (Image)sender;
                FileImageSource source = img.Source as FileImageSource;
                if (source.File == "fav.png")
                {
                    img.Source = "favselected.png";
                    await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=insert&userid=" + usr.user.id + "&itemid=" + img.TabIndex);
                }
                else
                {
                    img.Source = "fav.png";
                    await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=delete&userid=" + usr.user.id + "&itemid=" + img.TabIndex);
                }
            }
            else
            {
                await DisplayAlert("Not Logged in", "You need to login to use this feature", "OK");
            }
        }

        protected List<BindingItem> bindings = new List<BindingItem>();
        

        private async void list_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            var itemTypeObject = e.Item as BindingItem;
            List<BindingItem> i = (List<BindingItem>)list.ItemsSource;
            if (i.Last() == itemTypeObject)
            {
                morevendors.IsVisible = true;
                if (again)
                    again = await LoadMore();
                else
                    morevendors.IsVisible = false;
            }
        }

        public allVendors (int catid,string catname)
		{
            this._catid = catid;
            IsLoading = false;
            BindingContext = this;
            InitializeComponent ();
			backarrow.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};


			Pagetitle.Text = catname;
			selectedcatname = catname;
            page = 0;

            LoadPage(catid, new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>(), new List<int>());	
		}
        private bool isLoading;
        public bool IsLoading
        {
            get
            {
                return this.isLoading;
            }

            set
            {
                this.isLoading = value;
                RaisePropertyChanged("IsLoading");
            }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        public void RaisePropertyChanged(string propName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
        public async Task<bool> LoadMore()
        {
            page++;
            WordpressService service = new WordpressService();
            specificvendors = await service.GetItemsByFilterAsync(_catid, type.ToArray(), honeymoonexperienceInt.ToArray(), typeofserviceInt.ToArray(), capacityInt.ToArray(), setting.ToArray(), cateringservicesInt.ToArray(), typeoffurnitureInt.ToArray(), clienteleInt.ToArray(), clothingInt.ToArray(), beautyservicesInt.ToArray(), typeofmusiciansInt.ToArray(), city.ToArray(), itemlocationInt.ToArray(),page);
            //NumberOfSupplieres.Text = specificvendors.Count().ToString();
            var user = await GetUser();
            foreach (var post in specificvendors)
            {
                string favImg = "fav.png";
                if (user.user != null)
                {
                    Connect con = new Connect();
                    var s = await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=getsingle&userid=" + user.user.id + "&itemid=" + post.Id);
                    if (s == "1")
                    {
                        favImg = "favselected.png";
                    }
                    else
                    {
                        favImg = "fav.png";
                    }

                }
                string rendered = WebUtility.HtmlDecode(Regex.Replace(post.Content.Rendered, "<.*?>", ""));
                rendered = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                string title = WebUtility.HtmlDecode(post.Title.Rendered);
                string src = post.Embedded.WpFeaturedmedia != null ? post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl : "";
                BindingItem binding = new BindingItem { Desc = rendered, Title = title, item = post, Src = src, FavImg = favImg, Id = post.Id };
                bindings.Add(binding);
                /*Image img = new Image();
                try
                {
                    img.Source = post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl.Replace("https://", "http://");
                }
                catch (Exception ex)
                {

                }
                img.HorizontalOptions = LayoutOptions.FillAndExpand;
                list.Children.Add(img);

                Frame combo = new Frame();
                combo.BackgroundColor = Color.Red;
                combo.CornerRadius = 10;
                combo.Margin = new Thickness(15, -50, 0, 0);
                combo.HeightRequest = 10;
                combo.WidthRequest = 100;
                Label title = new Label();
                title.Text = post.Title.Rendered;
                title.FontAttributes = FontAttributes.Bold;
                title.Margin = new Thickness(10, 0, 10, 0);
                list.Children.Add(title);
                Label description = new Label();
                description.Margin = new Thickness(10, 0, 10, 0);
                string rendered = WebUtility.HtmlDecode(Regex.Replace(post.Content.Rendered, "<.*?>", ""));
                description.Text = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                list.Children.Add(description);
                Button but = new Button();
                but.Image = "moreinformation.png";
                but.HorizontalOptions = LayoutOptions.End;
                but.Margin = 0;
                but.Padding = 0;
                but.BorderColor = Color.Transparent;
                but.BackgroundColor = Color.Transparent;
                but.Margin = new Thickness(0, 0, 10, 0);
                list.Children.Add(but);
                //selectedpost = (IEnumerable<WordPressPCL.Models.Post>)post;
                but.Clicked += (s, e) => { Navigation.PushModalAsync(new selectedvendor(selectedcatname, post)); };
                */
            }
            morevendors.IsVisible = false;
            if (specificvendors.Count() > 0)
            {
                list.ItemsSource = new List<BindingItem>();
                list.ItemsSource = bindings;
                return true;
            }
            else
            {
                
                return false;
            }
            
        }
        public async void LoadPage(int catid, List<int> type, List<int> city, List<int> setting, List<int> cateringservicesInt, List<int> typeoffurnitureInt, List<int> clienteleInt, List<int> clothingInt, List<int> beautyservicesInt, List<int> typeofmusiciansInt, List<int> itemlocationInt, List<int> typeofserviceInt, List<int> capacityInt, List<int> honeymoonexperienceInt)
		{
            bindings = new List<BindingItem>();
            IsLoading = true;
            try
            {
                WordpressService service = new WordpressService();
                specificvendors = await service.GetItemsByFilterAsync(catid, type.ToArray(), honeymoonexperienceInt.ToArray(), typeofserviceInt.ToArray(), capacityInt.ToArray(), setting.ToArray(), cateringservicesInt.ToArray(), typeoffurnitureInt.ToArray(), clienteleInt.ToArray(), clothingInt.ToArray(), beautyservicesInt.ToArray(), typeofmusiciansInt.ToArray(), city.ToArray(), itemlocationInt.ToArray(), page);
                //NumberOfSupplieres.Text = specificvendors.Count().ToString();
                var user = await GetUser();
                foreach (var post in specificvendors)
                {
                    string favImg = "fav.png";
                    if (user.user != null)
                    {
                        Connect con = new Connect();
                        var s = await con.DownloadData("http://planmy.me/maizonpub-api/wishlist.php", "action=getsingle&userid=" + user.user.id + "&itemid=" + post.Id);
                        if (s == "1")
                        {
                            favImg = "favselected.png";
                        }
                        else
                        {
                            favImg = "fav.png";
                        }

                    }
                    //Image img = new Image();
                    //try
                    //{
                    //    img.Source = post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl.Replace("https://", "http://");
                    //}
                    //catch(Exception ex)
                    //{

                    //}
                    //img.HorizontalOptions = LayoutOptions.FillAndExpand;
                    //list.Margin = new Thickness(0, 15, 0, 0);
                    //list.Children.Add(img);

                    //Frame combo = new Frame();
                    //combo.BackgroundColor = Color.Red;
                    //combo.CornerRadius = 10;
                    //combo.Margin = new Thickness(15, -50, 0, 0);
                    //combo.HeightRequest = 10;
                    //combo.WidthRequest = 100;
                    //Label title = new Label();
                    //title.Text = post.Title.Rendered;
                    //title.FontAttributes = FontAttributes.Bold;
                    //title.Margin = new Thickness(10, 0, 10, 0);
                    //list.Children.Add(title);
                    //Label description = new Label();
                    //description.Margin = new Thickness(10, 0, 10, 0);
                    //string rendered = WebUtility.HtmlDecode(Regex.Replace(post.Content.Rendered, "<.*?>", ""));
                    //description.Text = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                    //list.Children.Add(description);
                    //Button but = new Button();
                    //but.Image = "moreinformation.png";
                    //but.HorizontalOptions = LayoutOptions.End;
                    //but.Margin = 0;
                    //but.Padding = 0;
                    //but.BorderColor = Color.Transparent;
                    //but.BackgroundColor = Color.Transparent;
                    //but.Margin = new Thickness(0, 0, 10, 0);
                    //list.Children.Add(but);
                    //Headerframe.HeightRequest = 40;
                    ////selectedpost = (IEnumerable<WordPressPCL.Models.Post>)post;
                    //but.Clicked += (s, e) => { Navigation.PushModalAsync(new selectedvendor(selectedcatname, post)); };
                    string rendered = WebUtility.HtmlDecode(Regex.Replace(post.Content.Rendered, "<.*?>", ""));
                    rendered = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                    string title = WebUtility.HtmlDecode(post.Title.Rendered);
                    string src = post.Embedded.WpFeaturedmedia!=null? post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl:"";
                    BindingItem binding = new BindingItem { Desc = rendered, Title = title, item = post, Src = src, FavImg = favImg, Id = post.Id  };
                    bindings.Add(binding);
                }
                list.ItemsSource = bindings;
            }
            catch(Exception ex)
            {

            }
            IsLoading = false;
        }

        private void filterBtn_Clicked(object sender, EventArgs e)
        {
            var filterPage = new VendorsFilter(_catid, ref type, ref city, ref setting, ref cateringservicesInt, ref typeoffurnitureInt, ref clienteleInt, ref clothingInt, ref beautyservicesInt, ref typeofmusiciansInt, ref itemlocationInt, ref typeofserviceInt, ref capacityInt, ref honeymoonexperienceInt);
            filterPage.OperationCompleted += FilterPage_OperationCompleted;
            Navigation.PushModalAsync(filterPage);
        }

        private void FilterPage_OperationCompleted(object sender, EventArgs e)
        {
            list.ItemsSource = new List<BindingItem>();
            page = 0;
            LoadPage(_catid, type, city, setting, cateringservicesInt, typeoffurnitureInt, clienteleInt, clothingInt, beautyservicesInt, typeofmusiciansInt, itemlocationInt, typeofserviceInt, capacityInt, honeymoonexperienceInt);
        }
    }
    public class BindingItem
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FavImg { get; set; }
        public WordPressPCL.Models.Item item { get; set; }
    }


}