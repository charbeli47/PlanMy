using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingleDeal : ContentPage
	{
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        Product prod;
        public SingleDeal (deals item)
		{
			InitializeComponent ();
            prod = item.product;
            Pagetitle.Text = item.title;
            
            carouselView.ItemsSource = item.product.images;
            HtmlWebViewSource source = new HtmlWebViewSource();
            source.Html = item.desc;
            desc.Source = source;

        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            BasketItem b = new BasketItem();
            List<BasketItem> lineItems = await b.Get();
            WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI("https://planmy.me/wp-json/wc/v2", Statics.ConsumerKey, Statics.ConsumerSecret);
            WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            var orderlineitem = new WooCommerceNET.WooCommerce.v2.OrderLineItem { };
            string prodprice = prod.price.ToString();
            if (prod.price != null && prod.price != 0)
            {

                var lineItem = new OrderLineItem { name = prod.name, price = prodprice, quantity = 1, product_id = prod.id, subtotal = prodprice, total = prodprice };
                if (lineItems.Where(x => x.OrderItem.product_id == prod.id).Count() == 0)
                {
                    lineItems = await b.AddItem(new BasketItem { OrderItem = lineItem, Product = prod });
                }
                else
                {
                    var item = lineItems.Where(x => x.OrderItem.product_id == prod.id).FirstOrDefault();
                    if (item != null)
                    {
                        item.OrderItem.quantity++;
                        decimal subtotal = decimal.Parse(item.OrderItem.price) * (int)item.OrderItem.quantity;
                        item.OrderItem.subtotal = subtotal.ToString();
                        item.OrderItem.total = subtotal.ToString();
                    }
                }
                //b.Save(lineItems);
                await DisplayAlert("Success", "You have added this offer to your basket", "CLOSE");
                await Navigation.PopModalAsync(true);
            }
            else
            {
                List<OrderLineItem> items = new List<OrderLineItem>();
                var lineItem = new OrderLineItem { name = prod.name, price = prodprice, quantity = 1, product_id = prod.id, subtotal = prodprice, total = prodprice };
                items.Add(lineItem);
                string userdata = await con.GetData("User");
                UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(userdata);
                int userId = cookie.user.id;
                string data = await con.DownloadData("http://planmy.me/maizonpub-api/users.php", "action=get&userid=" + userId);
                var user = Newtonsoft.Json.JsonConvert.DeserializeObject<ConfigUser>(data);
                var order = wc.Order;
                OrderBilling billing = new OrderBilling { address_1 = user.user_weddingcity, address_2 = "", city = user.user_weddingcity, country = "LB", email = user.email, first_name = user.first_name, last_name = user.last_name, state = "", postcode = "", phone = "" };
                try
                {
                    Order o = new Order { line_items = items, total = prod.price, customer_id = cookie.user.id, status = "processing", billing = billing, payment_method = "cybsawm", payment_method_title = "CyberSource Secure Acceptance", set_paid = true };
                    var addedOrder = await order.Add(o);

                    await DisplayAlert("Success", "Thank you for selecting this offer.\r\n We sent you an email please check it.", "CLOSE");
                    await Navigation.PopModalAsync(true);
                }
                catch (Exception ex)
                {
                    MessagingCenter.Subscribe<SingleDeal, string>(this, "Error",(s, arg) => {
                        Navigation.PopModalAsync(true);
                    });
                    MessagingCenter.Send<SingleDeal, string>(this, "Error", "An error occured, please try again later.");
                    await DisplayAlert("Error", "An error occured, please try again later.", "CLOSE");

                }
            }
        }
    }
}