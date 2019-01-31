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
	public partial class PaymentPage : ContentPage
	{
        protected Order addedOrder;
        protected bool paid = false;
        public PaymentPage (List<BasketItem> lineItems)
		{
			InitializeComponent ();
            LoadPage(lineItems);
		}

        private async void LoadPage(List<BasketItem> lineItems)
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            
            string userId = cookie.Id;
            var user = cookie;

            decimal total = 0;
            List<BasketItems> items = new List<BasketItems>();
            foreach (var item in lineItems)
            {
                total += item.OrderItem.TotalPrice;
                items.Add(item.OrderItem);
            }
            try
            {
                Order o = new Order { BasketItems = items, Total = total, UsersId = userId, OrderStatus = OrderStatus.Pending_Payment };
                string json = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                string resp = con.PostToServer(Statics.apiLink + "Orders", json);
                o = Newtonsoft.Json.JsonConvert.DeserializeObject<Order>(resp);
                string link = Statics.payLink+ "?OrderId={0}";
                link = string.Format(link, o.Id);
                paymentWebView.Source = link;
                paymentWebView.Navigated += PaymentWebView_Navigated;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Failure", "An error occured, please try again later.", "CLOSE");
                await Navigation.PopModalAsync();
            }
        }

        private void PaymentWebView_Navigated(object sender, WebNavigatedEventArgs e)
        {
            if(e.Url.Contains(Statics.payLink + "/Receipt"))
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
            new BasketItem().Clear();
            await Navigation.PopModalAsync();
        }

        private async void backarrow_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}