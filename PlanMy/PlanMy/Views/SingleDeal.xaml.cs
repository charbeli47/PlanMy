using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SingleDeal : ContentPage
	{
        ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
        Offers prod;
        public SingleDeal (deals item)
		{
			InitializeComponent ();
            prod = item.product;
            Pagetitle.Text = item.title;
            
            carouselView.ItemsSource = item.product.OffersGalleries;
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
            
            if (prod.Price != null && prod.Price != 0)
            {

                var lineItem = new BasketItems { OffersId = prod.Id, Offers = prod, Quantity = 1, TotalPrice = (decimal)prod.Price };
                if (lineItems.Where(x => x.OrderItem.OffersId == prod.Id).Count() == 0)
                {
                    lineItems = await b.AddItem(new BasketItem { OrderItem = lineItem, Product = prod });
                }
                else
                {
                    var item = lineItems.Where(x => x.OrderItem.OffersId == prod.Id).FirstOrDefault();
                    if (item != null)
                    {
                        item.OrderItem.Quantity++;
                        decimal subtotal = (decimal)item.OrderItem.Offers.Price * (int)item.OrderItem.Quantity;
                        item.OrderItem.TotalPrice = subtotal;
                    }
                }
                //b.Save(lineItems);
                await DisplayAlert("Success", "You have added this offer to your basket", "CLOSE");
                await Navigation.PopModalAsync(true);
            }
            else
            {
                List<BasketItems> items = new List<BasketItems>();
                var lineItem = new BasketItems { OffersId = prod.Id, Offers = prod, Quantity = 1, TotalPrice = (decimal)prod.Price };
                items.Add(lineItem);
                var usr = await con.GetData("User");
                Users cookie = new Users();
                if (!string.IsNullOrEmpty(usr))
                {
                    cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                    string userId = cookie.Id;
                    //OrderBilling billing = new OrderBilling { address_1 = user.user_weddingcity, address_2 = "", city = user.user_weddingcity, country = "LB", email = user.email, first_name = user.first_name, last_name = user.last_name, state = "", postcode = "", phone = "" };
                    try
                    {
                        Order o = new Order { BasketItems = items, Total = (decimal)prod.Price, Users = cookie, OrderStatus = OrderStatus.Pending_Payment, UsersId = cookie.Id };
                        string json = Newtonsoft.Json.JsonConvert.SerializeObject(o);
                        await con.PostToServer(Statics.apiLink + "Orders", json);

                        await DisplayAlert("Success", "Thank you for selecting this offer.\r\n We sent you an email please check it.", "CLOSE");
                        await Navigation.PopModalAsync(true);
                    }
                    catch (Exception ex)
                    {
                        MessagingCenter.Subscribe<SingleDeal, string>(this, "Error", (s, arg) =>
                        {
                            Navigation.PopModalAsync(true);
                        });
                        MessagingCenter.Send<SingleDeal, string>(this, "Error", "An error occured, please try again later.");
                        await DisplayAlert("Error", "An error occured, please try again later.", "CLOSE");

                    }
                }
            }
        }
    }
}