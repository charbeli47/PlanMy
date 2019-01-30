using PlanMy.Library;
using PlanMy.Models;
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
	public partial class DealsView : ContentView
	{
        protected List<Offers> hotdealsproducts;

         public DealsView ()
         {
             InitializeComponent ();
             LoadPage();
         }
         void LoadPage()
         {
            LoadCategories();
            WebClient client = new WebClient();
            string resp = client.DownloadString(Statics.apiLink + "Offers");
            List<Offers> offers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offers>>(resp);
            hotdealsproducts = offers.Where(x => x.OffersType == OffersType.HotDeals).ToList();
            List<deals> bundles = new List<deals>();
            foreach (var bundle in hotdealsproducts)
            {
                deals deal = new deals();
                deal.desc = bundle.Description;
                deal.img = Statics.MediaLink + bundle.Image;
                deal.product = bundle;
                deal.title = bundle.Title;
                deal.id = bundle.Id;
                bundles.Add(deal);
            }
            dealsList.FlowItemsSource = bundles;

        }
         private void LoadCategories()
         {
            WebClient client = new WebClient();
            var resp = client.DownloadString(Statics.apiLink + "Categories");
            var cats = Newtonsoft.Json.JsonConvert.DeserializeObject<List<VendorCategory>>(resp);
            var categories = cats.ToList();
            CategoriePicker.ItemsSource = categories;
         }
         private void dealsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
         {
             var item = (deals)e.Item;
             Navigation.PushModalAsync(new SingleDeal(item));
         }
         /*private async void LoadDeals(WooCommerceNET.WooCommerce.v2.WCObject wc, List<ProductTag> tags)
         {
             var hotdeals = tags.Where(x => x.slug == "hot-deal").FirstOrDefault();
             var p = wc.Product;
             var dic = new Dictionary<string, string>();
             dic.Add("tag", hotdeals.id.ToString());
             hotdealsproducts = await p.GetAll(dic);
             List<deals> listdeals = new List<deals>();
             foreach (var item in hotdealsproducts)
             {
                 deals d1 = new deals();
                 d1.img = item.images[0].src;
                 d1.title = item.name;
                 d1.desc = item.description;
                 d1.id = item.id;
                 d1.product = item;
                 listdeals.Add(d1);
             }
             dealsList.FlowItemsSource = listdeals;
             
        }*/

        private void CategoriePicker_SelectedIndexChanged(object sender, EventArgs e)
         {
             if(hotdealsproducts!=null)
             {
                 var picker = (Pickerarrow)sender;
                 var cat = (VendorCategory)picker.SelectedItem;
                 var filtered = hotdealsproducts.Where(x => x.OffersCategories.Where(z => z.Id == cat.Id).Count() > 0).ToList();
                 List<deals> listdeals = new List<deals>();
                 foreach (var item in filtered)
                 {
                     deals d1 = new deals();
                     d1.img = Statics.MediaLink + item.Image;
                     d1.title = item.Title;
                     d1.desc = item.Description;
                     d1.id = item.Id;
                     d1.product = item;
                     listdeals.Add(d1);
                 }
                 dealsList.FlowItemsSource = listdeals;
             }
         }
    }
}