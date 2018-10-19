using PlanMy.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class checklist : ContentPage
	{
		public checklist()
		{
			InitializeComponent();
			// for testing purposes ///
			searchsuppliers.GestureRecognizers.Add(
	new TapGestureRecognizer()
	{
		Command = new Command(() => { Navigation.PushModalAsync(new Vendors());})
	});
			for(int i = 0; i < 6; i++)
			{
				if (i == 0) {
					StackLayout firststack = createsupplierverticalstack("Elie Saab", "champagne.png");
					firststack.Margin = new Thickness(15, 0, 0, 0);
					Suppliersstack.Children.Add(firststack);
						}
				else
				{
					Suppliersstack.Children.Add(createsupplierverticalstack("Elie Saab", "champagne.png"));
				}
			}
		}
		public StackLayout createsupplierverticalstack(string nametxt,string imgurl)
		{
			StackLayout stack = new StackLayout();
			stack.Orientation = StackOrientation.Vertical;
			Image img = new Image();
			img.Source = imgurl;
			Label name = new Label();
			name.Text = nametxt;
			stack.Children.Add(img);
			stack.Children.Add(name);
			return stack;
		}
	}
}