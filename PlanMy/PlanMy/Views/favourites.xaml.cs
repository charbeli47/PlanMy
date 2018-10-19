using DLToolkit.Forms.Controls;
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
	public partial class favourites : ContentPage
	{
		List<favoritesobject> fo = new List<favoritesobject>();
		public favourites()
		{
			InitializeComponent();

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
			this.BindingContext = this;
			// just for testing purposes////
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
			

		}
	}

	public class favoritesobject{
		public string icon;
		public string name;
		public string categorie;
	}
}