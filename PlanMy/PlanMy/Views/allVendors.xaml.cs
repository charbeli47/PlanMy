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

        
        public string selectedcatname;
        protected int _catid;
        protected int page;
        protected bool again = true;
        public List<VendorTypeValue> types = new List<VendorTypeValue>();

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
            if (usr != null)
            {
                Image img = (Image)sender;
                FileImageSource source = img.Source as FileImageSource;
                if (source.File == "fav.png")
                {
                    img.Source = "favselected.png";
                    WishList list = new WishList { UserId = usr.Id, VendorItemId = img.TabIndex };
                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(list);
                    con.PostToServer(Statics.apiLink + "WishLists", json);
                }
                else
                {
                    img.Source = "fav.png";
                    con.DeleteFromServer(Statics.apiLink + "WishLists/" + img.TabIndex+"/"+usr.Id);
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

            LoadPage(catid, new List<VendorTypeValue>());	
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
        public async Task<bool> LoadMore()
        {
            page++;
            Connect con = new Connect();
            VendorItemSearch search = new VendorItemSearch { CategoryId = _catid, Values = types };
            string json = Newtonsoft.Json.JsonConvert.SerializeObject(search);
            string resp = con.PostToServer(Statics.apiLink + "VendorItems", json);
            var specificvendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(resp);
            //NumberOfSupplieres.Text = specificvendors.Count().ToString();
            var user = await GetUser();
            foreach (var post in specificvendors)
            {
                string favImg = "fav.png";
                if (user != null)
                {
                    var s = await con.DownloadData(Statics.apiLink+ "WishLists/"+post.Id+"/"+user.Id, "");
                    if (s == "1")
                    {
                        favImg = "favselected.png";
                    }
                    else
                    {
                        favImg = "fav.png";
                    }

                }
                string rendered = WebUtility.HtmlDecode(Regex.Replace(post.HtmlDescription, "<.*?>", ""));
                rendered = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                string title = WebUtility.HtmlDecode(post.Title);
                string src = post.Thumb != null ? Statics.MediaLink + post.Thumb : "";
                BindingItem binding = new BindingItem { Desc = rendered, Title = title, item = post, Src = src, FavImg = favImg, Id = post.Id };
                bindings.Add(binding);
                
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
        public async void LoadPage(int catid, List<VendorTypeValue> types)
		{
            bindings = new List<BindingItem>();
            IsLoading = true;
            try
            {
                Connect con = new Connect();
                VendorItemSearch search = new VendorItemSearch { CategoryId = _catid, Values = types };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(search);
                string resp = con.PostToServer(Statics.apiLink + "VendorItems", json);
                var specificvendors = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorItem>>(resp);
                var user = await GetUser();
                foreach (var post in specificvendors)
                {
                    string favImg = "fav.png";
                    if (user != null)
                    {
                        var s = await con.DownloadData(Statics.apiLink + "WishLists/" + post.Id + "/" + user.Id, "");
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
                    string rendered = WebUtility.HtmlDecode(Regex.Replace(post.HtmlDescription, "<.*?>", ""));
                    rendered = rendered.Length > 100 ? rendered.Substring(0, 100) + "more..." : rendered;
                    string title = WebUtility.HtmlDecode(post.Title);
                    string src = post.Thumb != null ? Statics.MediaLink + post.Thumb : "";
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
            var filterPage = new VendorsFilter(_catid, ref types);
            filterPage.OperationCompleted += FilterPage_OperationCompleted;
            Navigation.PushModalAsync(filterPage);
        }

        private void FilterPage_OperationCompleted(object sender, EventArgs e)
        {
            list.ItemsSource = new List<BindingItem>();
            page = 0;
            LoadPage(_catid, types);
        }
    }
    public class BindingItem
    {
        public int Id { get; set; }
        public string Src { get; set; }
        public string Title { get; set; }
        public string Desc { get; set; }
        public string FavImg { get; set; }
        public VendorItem item { get; set; }
    }


}