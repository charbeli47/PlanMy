using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
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
        void LoadPage()
        {
            WebClient client = new WebClient();
            string resp = client.DownloadString(Statics.apiLink + "Offers");
            List<Offers> offers = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Offers>>(resp);
            var bundlesproducts = offers.Where(x => x.OffersType == OffersType.Bundles);
            List<deals> bundles = new List<deals>();
            foreach (var bundle in bundlesproducts)
            {
                deals deal = new deals();
                deal.desc = bundle.Description;
                deal.img = Statics.MediaLink+bundle.Image;
                deal.product = bundle;
                deal.title = bundle.Title;
                deal.id = bundle.Id;
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