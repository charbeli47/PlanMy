using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class OrdersView : ContentView
	{
		public OrdersView ()
		{
			InitializeComponent ();
            LoadPage();
        }
        async void LoadPage()
        {
            Connect con = new Connect();
             var usr = await con.GetData("User");
             Users cookie = new Users();
             if (!string.IsNullOrEmpty(usr))
             {
                 cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                WebClient client = new WebClient();
                string resp = client.DownloadString(Statics.apiLink + "Orders?UserId=" + cookie.Id);
                var orders = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Order>>(resp);
                 orders = orders.Where(x => x.OrderStatus == OrderStatus.Completed || x.OrderStatus == OrderStatus.Processing).ToList();
                 List<BasketItem> basket = new List<BasketItem>();
                 int itemsCount = 0;
                 List<RenderedItem> items = new List<RenderedItem>();


                 foreach (var order in orders)
                 {
                     foreach (var item in order.BasketItems)
                     {
                        var r = client.DownloadString(Statics.apiLink + "Offers/" + item.OffersId);
                        var prod = Newtonsoft.Json.JsonConvert.DeserializeObject<Offers>(r);
                         var rendered = new RenderedItem();
                         rendered.title = prod.Title;
                         rendered.description = prod.Description;
                         rendered.img = Statics.MediaLink + prod.Image;
                         rendered.price = (prod.SalePrice != null || prod.SalePrice != 0) ? prod.SalePrice : prod.Price;
                         items.Add(rendered);
                     }
                     itemsCount += order.BasketItems.Count;
                 }
                 BasketListView.ItemsSource = items;
             }
        }
        private void BasketListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}