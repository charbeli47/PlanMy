using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class PaymentPage : ContentPage
	{
        protected Order addedOrder;
        protected WooCommerceNET.WooCommerce.v2.WCObject wc;
        protected bool paid = false;
        public PaymentPage (List<BasketItem> lineItems)
		{
			InitializeComponent ();
            LoadPage(lineItems);
		}

        private async void LoadPage(List<BasketItem> lineItems)
        {
            Connect con = new Connect();
            string userdata = await con.GetData("User");
            UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(userdata);
            WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI("https://www.planmy.me/wp-json/wc/v2/", Statics.ConsumerKey, Statics.ConsumerSecret);
            wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            int userId = cookie.user.id;
            var user = cookie.configUsr;

            decimal total = 0;
            List<OrderLineItem> items = new List<OrderLineItem>();
            foreach (var item in lineItems)
            {
                total += !string.IsNullOrEmpty(item.OrderItem.total) ? decimal.Parse(item.OrderItem.total) : 0;
                items.Add(item.OrderItem);
            }
            var order = wc.Order;
            OrderBilling billing = new OrderBilling { address_1 = user.user_weddingcity, address_2 = "", city = user.user_weddingcity, country = "LB", email = user.email, first_name = user.first_name, last_name = user.last_name, state = "", postcode = "", phone = "" };
            try
            {
                Order o = new Order { line_items = items, total = total, customer_id = cookie.user.id, status = "pending", billing = billing, payment_method = "cybsawm", payment_method_title = "CyberSource Secure Acceptance", set_paid = false };
                addedOrder = await order.Add(o);

                string link = "https://planmy.me/maizonpub-api/pay/payment_form.php?reference_number={0}&bill_to_forename={1}&bill_to_surname={2}&bill_to_phone={3}&bill_to_email={4}&bill_to_address_line1={5}&bill_to_address_city={6}&bill_to_address_country={7}&amount={8}";
                link = string.Format(link, addedOrder.number, addedOrder.billing.first_name, addedOrder.billing.last_name, addedOrder.billing.phone, addedOrder.billing.email, addedOrder.billing.address_1, addedOrder.billing.city, addedOrder.billing.country, addedOrder.total);
                paymentWebView.Source = link;
                paymentWebView.Navigated += PaymentWebView_Navigated;
                /*paymentWebView.LoadRequest(new NSUrlRequest(NSUrl.FromString(link)));
                paymentWebView.ShouldStartLoad = myHandler;
                paymentWebView.LoadFinished += PaymentWebView_LoadFinished;*/
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", "An error occured, please try again later.", "CLOSE");
                await Navigation.PopModalAsync();
            }
        }

        private void PaymentWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if(e.Url.Contains("/maizonpub-api/pay/receipt.php"))
            {
                string req = e.Url.Substring(e.Url.LastIndexOf("?")+1);
                string[] query = req.Split('&');
                Dictionary<string, string> dic = new Dictionary<string, string>();
                foreach (string q in query)
                {
                    string[] split = q.Split('=');
                    dic.Add(split[0], split[1]);
                }
                if (dic.ContainsKey("reason_code"))
                {
                    if (dic["reason_code"] == "100" || dic["reason_code"] == "487")
                    {
                        ProcessOrder();
                    }
                }
            }
        }

        async void ProcessOrder()
        {
            var order = wc.Order;
            addedOrder.status = "processing";
            addedOrder.set_paid = true;
            addedOrder = await order.Update((int)addedOrder.id, addedOrder);
            paid = true;

            new BasketItem().Clear();
            await Navigation.PopModalAsync();
        }

        private async void backarrow_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}