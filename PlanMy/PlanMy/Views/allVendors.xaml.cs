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
        public List<int> type = new List<int>(), city = new List<int>(), setting = new List<int>(), cateringservicesInt = new List<int>(), typeoffurnitureInt = new List<int>(), clienteleInt = new List<int>(), clothingInt = new List<int>(), beautyservicesInt = new List<int>(), typeofmusiciansInt = new List<int>(), itemlocationInt = new List<int>(), typeofserviceInt = new List<int>(), capacityInt = new List<int>(), honeymoonexperienceInt = new List<int>();

        private void ScrollView_Scrolled(object sender, ScrolledEventArgs e)
        {
            if (e.ScrollY >= myScrollView.ContentSize.Height - myScrollView.Height - 30)
            {
                try
                {
                    LoadMore();
                }
                catch { }
            }
        }

        private void list_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        {
            //var itemTypeObject = e.Item as Item;
            //IEnumerable<Item> i = (IEnumerable<Item>)list.ItemsSource;
            //if (i.ToList().Last() == itemTypeObject)
            //{
            //    page++;
            //}
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
            myScrollView.HeightRequest = Application.Current.MainPage.Height - 100;

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
        public async void LoadMore()
        {
            page++;
            WordpressService service = new WordpressService();
            specificvendors = await service.GetItemsByFilterAsync(_catid, type.ToArray(), honeymoonexperienceInt.ToArray(), typeofserviceInt.ToArray(), capacityInt.ToArray(), setting.ToArray(), cateringservicesInt.ToArray(), typeoffurnitureInt.ToArray(), clienteleInt.ToArray(), clothingInt.ToArray(), beautyservicesInt.ToArray(), typeofmusiciansInt.ToArray(), city.ToArray(), itemlocationInt.ToArray(),page);
            //NumberOfSupplieres.Text = specificvendors.Count().ToString();
            foreach (var post in specificvendors)
            {
                Image img = new Image();
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

            }
        }
        public async void LoadPage(int catid, List<int> type, List<int> city, List<int> setting, List<int> cateringservicesInt, List<int> typeoffurnitureInt, List<int> clienteleInt, List<int> clothingInt, List<int> beautyservicesInt, List<int> typeofmusiciansInt, List<int> itemlocationInt, List<int> typeofserviceInt, List<int> capacityInt, List<int> honeymoonexperienceInt)
		{
            IsLoading = true;
            list.Children.Clear();
            try
            {
                WordpressService service = new WordpressService();
                specificvendors = await service.GetItemsByFilterAsync(catid, type.ToArray(), honeymoonexperienceInt.ToArray(), typeofserviceInt.ToArray(), capacityInt.ToArray(), setting.ToArray(), cateringservicesInt.ToArray(), typeoffurnitureInt.ToArray(), clienteleInt.ToArray(), clothingInt.ToArray(), beautyservicesInt.ToArray(), typeofmusiciansInt.ToArray(), city.ToArray(), itemlocationInt.ToArray(), page);
                //NumberOfSupplieres.Text = specificvendors.Count().ToString();
                foreach (var post in specificvendors)
                {
                    Image img = new Image();
                    try
                    {
                        img.Source = post.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl.Replace("https://", "http://");
                    }
                    catch(Exception ex)
                    {

                    }
                    img.HorizontalOptions = LayoutOptions.FillAndExpand;
                    list.Margin = new Thickness(0, 15, 0, 0);
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
                    Headerframe.HeightRequest = 40;
                    //selectedpost = (IEnumerable<WordPressPCL.Models.Post>)post;
                    but.Clicked += (s, e) => { Navigation.PushModalAsync(new selectedvendor(selectedcatname, post)); };

                }
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
            LoadPage(_catid, type, city, setting, cateringservicesInt, typeoffurnitureInt, clienteleInt, clothingInt, beautyservicesInt, typeofmusiciansInt, itemlocationInt, typeofserviceInt, capacityInt, honeymoonexperienceInt);
        }
    }



}