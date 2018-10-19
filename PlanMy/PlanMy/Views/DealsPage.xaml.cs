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
	public partial class DealsPage : ContentPage
	{
		public DealsPage()
		{
			InitializeComponent();
			// go to pages on click//

			dealsbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new DealsPage());
			};
			bundlesbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new bundels());
			};


			List<deals> listdeals = new List<deals>();
			deals d1 = new deals();
			deals d2 = new deals();
			deals d3 = new deals();
			listdeals.Add(d1);
			listdeals.Add(d2);
			listdeals.Add(d3);
			dealsList.FlowItemsSource = listdeals;

			//
			CategoriePicker.SelectedIndex = 0;


		}
	}

	// for testing purposes//
	public class deals{
		public string img;
		public string title;
		public string desc;
	}
}