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
        //commit from charbel public IEnumerable<WordPressPCL.Models.ItemCategory> vendors;
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

                /*commit from charbel var selectedvendor = e.SelectedItem as WordPressPCL.Models.ItemCategory;

                 if (selectedvendor == null)
                     return;
                 ((ListView)sender).SelectedItem = null;
                 //var newpage = new allVendors(selectedvendor.Id,selectedvendor.Name);
                 Navigation.PushModalAsync(new allVendors(selectedvendor.Id, selectedvendor.Name));*/
             };
         }
         private void Entry_TextChanged(object sender, TextChangedEventArgs e)
         {
            /*commit from charbel VendorsListView.BeginRefresh();

            if (string.IsNullOrWhiteSpace(e.NewTextValue))
                VendorsListView.ItemsSource = vendors;
            else
                VendorsListView.ItemsSource = vendors.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToString().ToLower()));

            VendorsListView.EndRefresh();*/
        }

        async void LoadVendors()
        {

            //var vendors = service.GetItemCategoriesAsync();
            /*commit from charbel vendors = await service.GetItemCategoriesAsync();
            foreach(var vendor in vendors)
            {
                vendor.Name = WebUtility.HtmlDecode(vendor.Name);
            }
            VendorsListView.ItemsSource = vendors;*/
        }
    }
}