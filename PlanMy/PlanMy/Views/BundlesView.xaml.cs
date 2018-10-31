using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WooCommerceNET.WooCommerce.v2;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class BundlesView : ContentView
	{
		public BundlesView ()
		{
			InitializeComponent ();
            LoadPage();
		}
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            ForceLayout();
            base.OnPropertyChanged(propertyName);
        }
        async void LoadPage()
        {
            WooCommerceNET.RestAPI rest = new WooCommerceNET.RestAPI(Statics.WooApi, Statics.ConsumerKey, Statics.ConsumerSecret);
            WooCommerceNET.WooCommerce.v2.WCObject wc = new WooCommerceNET.WooCommerce.v2.WCObject(rest);
            var tags = await LoadTags(wc);
            LoadBundles(wc, tags);

        }
        private async Task<List<ProductTag>> LoadTags(WooCommerceNET.WooCommerce.v2.WCObject wc)
        {
            var t = wc.Tag;
            var tags = await t.GetAll();
            return tags;

        }
        private async void LoadBundles(WCObject wc, List<ProductTag> tags)
        {
            var bundlest = tags.Where(x => x.slug == "bundle").FirstOrDefault();
            var p = wc.Product;
            var dic = new Dictionary<string, string>();
            dic.Add("tag", bundlest.id.ToString());
            var bundlesproducts = await p.GetAll(dic);
            List<deals> bundles = new List<deals>();
            foreach (var bundle in bundlesproducts)
            {
                deals deal = new deals();
                deal.desc = bundle.description;
                deal.img = bundle.images[0].src;
                deal.product = bundle;
                deal.title = bundle.name;
                deal.id = bundle.id;
                bundles.Add(deal);
            }
            bundlesList.FlowItemsSource = bundles;
        }
        private void dealsList_FlowItemTapped(object sender, ItemTappedEventArgs e)
        {
            var item = (deals)e.Item;
            Navigation.PushModalAsync(new SingleDeal(item));
        }
    }
}