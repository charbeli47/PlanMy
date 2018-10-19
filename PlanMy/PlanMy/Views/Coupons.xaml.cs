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
	public partial class Coupons : ContentPage
	{
		public Coupons ()
		{
			InitializeComponent ();

			StackLayout layout = new StackLayout();
			layout.Orientation = StackOrientation.Vertical;
			layout.Margin = new Thickness(15,15,15,0);

			Label title = new Label();
			title.Text = "Winter Wedding Package";
			title.TextColor = Color.Black;
			title.FontSize = 20;
			title.FontAttributes = FontAttributes.Bold;
			layout.Children.Add(title);

			StackLayout discountlayout = new StackLayout();
			discountlayout.Orientation = StackOrientation.Horizontal;
			Image img = new Image();
			img.Source = "discount.png";
			img.HorizontalOptions = LayoutOptions.Start;

			Label date = new Label();
			date.Text = "Expires on 31/01/2019";
			date.HorizontalOptions = LayoutOptions.Center;
			date.Margin = new Thickness(10, 0, 0, 0);
			date.TextColor = Color.Black;

		discountlayout.Children.Add(img);
		discountlayout.Children.Add(date);
		layout.Children.Add(discountlayout);

			Image imge = new Image();
			imge.Source = "coupon.png";
			imge.HorizontalOptions = LayoutOptions.FillAndExpand;
			imge.Margin = new Thickness(0, 25, 0, 0);
			layout.Children.Add(imge);

			Button but = new Button();
			but.Image = "recievecoupon.png";
			but.BackgroundColor = Color.Transparent;
			but.Margin = new Thickness(0, -15, 0, 0);

			layout.Children.Add(but);

			Label description = new Label();
			description.Text = "For weddings in winter of 2018 and 2019, recieve a 30% discount on our indoor venues";
			description.TextColor = Color.Black;
			description.FontAttributes = FontAttributes.Bold;
			description.Margin = new Thickness(0, -15, 0, 0);

			layout.Children.Add(description);


			content.Children.Add(layout);
			//Couponslist.Margin = new Thickness(15, 0, 15, 0);
		}
	}
}