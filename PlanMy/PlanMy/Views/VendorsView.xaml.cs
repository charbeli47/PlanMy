using PlanMy.Library;
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
	public partial class VendorsView : ContentView
	{
        public IEnumerable<VendorCategory> vendors;
        public VendorsView ()
		{
			InitializeComponent ();
            LoadVendors();

            VendorsListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
            {

                if (e.SelectedItem == null)
                {
                    return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
                }

                var selectedvendor = e.SelectedItem as VendorCategory;

                 if (selectedvendor == null)
                     return;
                 ((ListView)sender).SelectedItem = null;
                 //var newpage = new allVendors(selectedvendor.Id,selectedvendor.Name);
                 Navigation.PushModalAsync(new allVendors(selectedvendor.Id, selectedvendor.Title));
             };
         }
         private void Entry_TextChanged(object sender, TextChangedEventArgs e)
         {
            VendorsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                VendorsListView.ItemsSource = vendors;
            else
                VendorsListView.ItemsSource = vendors.Where(i => i.Title.ToLower().Contains(e.NewTextValue.ToString().ToLower()));

            VendorsListView.EndRefresh();
        }

        async void LoadVendors()
        {

            //var vendors = service.GetItemCategoriesAsync();
            WebClient client = new WebClient();
            var resp = client.DownloadString(Statics.apiLink + "Categories");
            vendors = Newtonsoft.Json.JsonConvert.DeserializeObject<IEnumerable<VendorCategory>>(resp);
            foreach(var vendor in vendors)
            {
                vendor.Title = WebUtility.HtmlDecode(vendor.Title);
                vendor.Image = Statics.MediaLink + vendor.Image;
            }
            VendorsListView.ItemsSource = vendors;
        }
    }
}