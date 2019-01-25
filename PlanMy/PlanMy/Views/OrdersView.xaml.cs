using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
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
            /*commit from charbel Connect con = new Connect();
             var usr = await con.GetData("User");
             UserCookie cookie = new UserCookie();
             if (!string.IsNullOrEmpty(usr))
             {
                 cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
                 WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI("https://planmy.me/wp-json/wc/v2/", Statics.ConsumerKey, Statics.ConsumerSecret);
                 WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
                 int userId = cookie.user.id;
                 var dic = new Dictionary<string, string>();
                 dic.Add("customer", userId.ToString());
                 var orders = await wc.Order.GetAll(dic);
                 orders = orders.Where(x => x.status == "completed" || x.status == "processing").ToList();
                 List<BasketItem> basket = new List<BasketItem>();
                 int itemsCount = 0;
                 List<RenderedItem> items = new List<RenderedItem>();


                 foreach (var order in orders)
                 {
                     foreach (var item in order.line_items)
                     {
                         var prod = await wc.Product.Get((int)item.product_id);
                         var rendered = new RenderedItem();
                         rendered.title = prod.name;
                         rendered.description = prod.description;
                         rendered.img = prod.images[0].src;
                         rendered.price = prod.price;
                         items.Add(rendered);
                     }
                     itemsCount += order.line_items.Count;
                 }
                 BasketListView.ItemsSource = items;
             }*/
        }
        private void BasketListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            ((ListView)sender).SelectedItem = null;
        }
    }
}