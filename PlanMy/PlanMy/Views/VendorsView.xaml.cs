﻿using PlanMy.Library;
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
	public partial class VendorsView : ContentView
	{
        public IEnumerable<WordPressPCL.Models.ItemCategory> vendors;
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

                var selectedvendor = e.SelectedItem as WordPressPCL.Models.ItemCategory;

                if (selectedvendor == null)
                    return;
                ((ListView)sender).SelectedItem = null;
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
                VendorsListView.ItemsSource = vendors.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToString().ToLower()));

            VendorsListView.EndRefresh();
        }

        async void LoadVendors()
        {

            WordpressService service = new WordpressService();
            //var vendors = service.GetItemCategoriesAsync();
            vendors = await service.GetItemCategoriesAsync();
            VendorsListView.ItemsSource = vendors;
        }
    }
}