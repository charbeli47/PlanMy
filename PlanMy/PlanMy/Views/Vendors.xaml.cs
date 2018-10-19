using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using PlanMy.Library;
using System.Collections.ObjectModel;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Vendors : ContentPage
	{
		public IEnumerable<WordPressPCL.Models.ItemCategory> vendors;
		public Vendors ()
		{
			InitializeComponent ();

			search.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new Vendors());
			};

			favorites.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new favourites());
			};

			message.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new messages());
			};

			WordpressService service = new WordpressService();
			//var vendors = service.GetItemCategoriesAsync();
			Task.Run(async () =>
			{
				 vendors=await service.GetItemCategoriesAsync();
				VendorsListView.ItemsSource = vendors;
			});

			VendorsListView.ItemSelected += (object sender, SelectedItemChangedEventArgs e) =>
			{

				if (e.SelectedItem == null)
				{
					return; //ItemSelected is called on deselection, which results in SelectedItem being set to null
				}

				var selectedvendor = e.SelectedItem as WordPressPCL.Models.ItemCategory;

				if (selectedvendor == null)
					return;
				//var newpage = new allVendors(selectedvendor.Id,selectedvendor.Name);
				Navigation.PushModalAsync(new allVendors(selectedvendor.Id, selectedvendor.Name));
			};



		}

		private void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			VendorsListView.BeginRefresh();

			if (string.IsNullOrWhiteSpace(e.NewTextValue))
				VendorsListView.ItemsSource = vendors;
			else
				VendorsListView.ItemsSource = vendors.Where(i => i.Name.Contains(e.NewTextValue));

			VendorsListView.EndRefresh();
		}
	}
}