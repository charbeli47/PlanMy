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
	public partial class BasketView : ContentPage
	{
        public List<BasketItem> lineItems;
        public BasketView ()
		{
			InitializeComponent ();
            LoadPage();
		}

        private async void LoadPage()
        {
            var item = new BasketItem();
            lineItems = await item.Get();
            List<RenderedItem> items = new List<RenderedItem>();
            foreach(var line in lineItems)
            {
                var rendered = new RenderedItem();
                rendered.title = line.Product.Title;
                rendered.description = line.Product.Description;
                rendered.img = line.Product.OffersGalleries[0].Image;
                rendered.price = line.Product.Price;
                items.Add(rendered);
            }
            BasketListView.ItemsSource = items;
        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }

        private void proceedBtn_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new PaymentPage(lineItems));
        }
    }
    public class RenderedItem
    {
        public string title { get; set; }
        public string img { get; set; }
        public string description { get; set; }
        public decimal? price { get; set; }
    }
}