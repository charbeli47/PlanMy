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
	public partial class bundels : ContentPage
	{
		public bundels()
		{
			InitializeComponent();
			// go to pages//
			dealsbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new DealsPage());
			};
			bundlesbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new bundels());
			};

			for (int i = 0; i < 3; i++)
			{
				StackLayout layout = new StackLayout();
				layout.Orientation = StackOrientation.Vertical;

				Image img = new Image();
				img.Source = "bundledealerpic.png";
				img.HorizontalOptions = LayoutOptions.FillAndExpand;

				Label title = new Label();
				title.Text = "The 5 IN 1";
				title.TextColor = Color.Black;
				title.Margin = new Thickness(0, -5, 0, 0);
				title.FontAttributes = FontAttributes.Bold;
				title.HorizontalOptions = LayoutOptions.Center;

				Label desc = new Label();
				desc.Text = "5 in 1 Full Party Pack";
				desc.HorizontalOptions = LayoutOptions.Center;
				desc.Margin = new Thickness(0, -5, 0, 0);
				desc.HorizontalOptions = LayoutOptions.Center;

				layout.Children.Add(img);
				layout.Children.Add(title);
				layout.Children.Add(desc);

				Button buynow = new Button();
				buynow.Image = "buynow.png";
				buynow.BackgroundColor = Color.Transparent;
				buynow.Margin = new Thickness(0, -15, 0, 0);
				
				layout.Children.Add(buynow);

				bundleslist.Children.Add(layout);
				bundleslist.Margin = new Thickness(0, 10, 0, 0);
			}
		}

		}
	}


