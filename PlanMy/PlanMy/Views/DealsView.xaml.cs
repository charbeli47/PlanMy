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
        /*commit from charbel protected List<Product> hotdealsproducts;

         public DealsView ()
         {
             InitializeComponent ();
             LoadPage();
         }
         async void LoadPage()
         {
             WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI(Statics.WooApi, Statics.ConsumerKey, Statics.ConsumerSecret);
             WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
             var tags = await LoadTags(wc);
             LoadCategories(wc);
             LoadDeals(wc, tags);

         }
         private async void LoadCategories(WCObject wc)
         {
             var cat = wc.Category;
             var categories = await cat.GetAll();
             foreach (var c in categories)
                 c.name = WebUtility.HtmlDecode(c.name);
             CategoriePicker.ItemsSource = categories;
         }
         private async Task<List<ProductTag>> LoadTags(WooCommerceNET.WooCommerce.v2.WCObject wc)
         {
             var t = wc.Tag;
             var tags = await t.GetAll();
             return tags;

         }*/
         private void dealsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
         {
             /*var item = (deals)e.Item;
             Navigation.PushModalAsync(new SingleDeal(item));*/
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
             /*if(hotdealsproducts!=null)
             {
                 var picker = (Pickerarrow)sender;
                 var cat = (ProductCategory)picker.SelectedItem;
                 var filtered = hotdealsproducts.Where(x => x.categories.Where(z => z.id == cat.id).Count() > 0).ToList();
                 List<deals> listdeals = new List<deals>();
                 foreach (var item in filtered)
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
         }
    }
}