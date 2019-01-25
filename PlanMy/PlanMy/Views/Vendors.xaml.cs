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
using PlanMy.ViewModels;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Vendors : ContentPage
	{
		public IEnumerable<VendorCategory> vendors;
		public List<message> listmsgs;

		public Vendors ()
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);
            List<Views> vs = new List<Views>();
            VendorsView v = new VendorsView();
            favoriteView f = new favoriteView();
            messageView m = new messageView();
            vs.Add(new Views { content = v });
            vs.Add(new Views { content = f });
            vs.Add(new Views { content = m });
            carouselView.ItemsSource = vs;
            // for messages//
            /*listmsgs = new List<message>();
			message msg = new message();
			msg.title = "Candid Image";
			msg.msg = "Hello thanks for ....";
			msg.time = "-23h";
			message msg2 = new message();
			msg2.title = "Candid Image";
			msg2.msg = "Hello thanks for ....";
			msg2.time = "-23h";
			listmsgs.Add(msg);
			listmsgs.Add(msg2);
			MessagesListView.ItemsSource = listmsgs;


			//for favorites//
			List<favoritesobject> fo = new List<favoritesobject>();
			favoritesobject object1 = new favoritesobject();
			object1.icon = "";
			object1.name = "CHAMPAGNE";
			object1.categorie = "Clothing & Accessories";

			favoritesobject object2 = new favoritesobject();
			object1.icon = "";
			object1.name = "FOUR SEASONS";
			object1.categorie = "VENUES";

			favoritesobject object3 = new favoritesobject();
			object1.icon = "";
			object1.name = "NAJI OSTA";
			object1.categorie = "ENTERTAINMENT";

			favoritesobject object4 = new favoritesobject();
			object1.icon = "";
			object1.name = "CHAMPAGNE";
			object1.categorie = "Clothing & Accessories";

			favoritesobject object5 = new favoritesobject();
			object1.icon = "";
			object1.name = "CHAMPAGNE";
			object1.categorie = "Clothing & Accessories";

			fo.Add(object1);
			fo.Add(object2);
			fo.Add(object3);
			fo.Add(object4);
			fo.Add(object5);
			FavoritesList.FlowItemsSource = fo;
			//////

            */


            search.Clicked += (object sender, EventArgs e) =>
			{
				search.Image = "searchblue.png";
				favorites.Image = "favorites.png";
				message.Image = "messages.png";
                carouselView.Position = 0;
                //messageview.IsVisible = false;
                //favoriteview.IsVisible = false;
                //vendorview.IsVisible = true;
            };

			favorites.Clicked += (object sender, EventArgs e) =>
			{
				search.Image = "search.png";
				favorites.Image = "favoritesblue.png";
				message.Image = "messages.png";
                carouselView.Position = 1;
                //messageview.IsVisible = false;
                //favoriteview.IsVisible = true;
                //vendorview.IsVisible = false;
            };

			message.Clicked += (object sender, EventArgs e) =>
			{
				search.Image = "search.png";
				favorites.Image = "favorites.png";
				message.Image = "messagesblue.png";
                carouselView.Position = 2;
                //messageview.IsVisible = true;
                //favoriteview.IsVisible = false;
                //vendorview.IsVisible = false;
            };


			/*LoadVendors();

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
            */


		}

        private void carouselView_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            switch (e.NewValue)
            {
                case 0:
                    search.Image = "searchblue.png";
                    favorites.Image = "favorites.png";
                    message.Image = "messages.png";
                    break;
                case 1:
                    search.Image = "search.png";
                    favorites.Image = "favoritesblue.png";
                    message.Image = "messages.png";
                    break;
                case 2:
                    search.Image = "search.png";
                    favorites.Image = "favorites.png";
                    message.Image = "messagesblue.png";
                    break;
            }
        }

        //private void Entry_TextChanged(object sender, TextChangedEventArgs e)
        //{
        //	VendorsListView.BeginRefresh();

        //	if (string.IsNullOrWhiteSpace(e.NewTextValue))
        //		VendorsListView.ItemsSource = vendors;
        //	else
        //		VendorsListView.ItemsSource = vendors.Where(i => i.Name.ToLower().Contains(e.NewTextValue.ToString().ToLower()));

        //	VendorsListView.EndRefresh();
        //}

        //async void LoadVendors()
        //{

        //	WordpressService service = new WordpressService();
        //	//var vendors = service.GetItemCategoriesAsync();
        //	vendors = await service.GetItemCategoriesAsync();
        //	VendorsListView.ItemsSource = vendors;
        //}


    }

	
}