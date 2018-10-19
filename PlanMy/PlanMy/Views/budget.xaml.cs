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
	public partial class budget : ContentPage
	{
		public budget()
		{
			InitializeComponent();

			budgetbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new budget());
			};

			checklistbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new Planning());
			};

			guestbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new guests());
			};
			supplierbut.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new favourites());
			};



			StackLayout table1 = createtablerow("Venues", "location.png");
			StackLayout table2 = createtablerow("Lighting & sound", "location.png");
			table1.Children.Add(createsupplierrowintable("Domaine de Zekrit", "25 000","10 000"));

			table2.Children.Add(createsupplierrowintable("Wicked Solutions", "5 000","1000"));
			table2.Children.Add(createsupplierrowintable("Basement Music", "2 000","200"));

			content.Children.Add(table1);
			content.Children.Add(createseperatorbetweentables());
			content.Children.Add(table2);
			content.Children.Add(createseperatorbetweentables());
		}
			public StackLayout createtablerow(string namet, string icon)
			{
				StackLayout tablelayout = new StackLayout();
				tablelayout.Orientation = StackOrientation.Vertical;

				StackLayout rowlayout = new StackLayout();
				rowlayout.Orientation = StackOrientation.Horizontal;
				rowlayout.Margin = new Thickness(15, 0, 15, 0);

				Image plusimg = new Image();
				plusimg.Source = icon;
				plusimg.HorizontalOptions = LayoutOptions.Start;
				plusimg.Margin = new Thickness(0, 0, 25, 0);
				plusimg.VerticalOptions = LayoutOptions.Center;
				rowlayout.Children.Add(plusimg);

				StackLayout vlayout = new StackLayout();
				vlayout.Orientation = StackOrientation.Vertical;

				Label nametable = new Label();
				nametable.Text = namet;
				nametable.FontSize = 18;
				nametable.TextColor = Color.Black;
				nametable.VerticalOptions = LayoutOptions.Center;
			nametable.HorizontalOptions = LayoutOptions.FillAndExpand;
			nametable.Margin=new Thickness(0, 10, 0, 0);
				nametable.FontAttributes = FontAttributes.Bold;


			vlayout.Children.Add(nametable);

				vlayout.HorizontalOptions = LayoutOptions.FillAndExpand;
				rowlayout.Children.Add(vlayout);


				Button img = new Button();
				img.Image = "downarrow.png";
				img.HorizontalOptions = LayoutOptions.End;
				img.VerticalOptions = LayoutOptions.Center;
				img.BackgroundColor = Color.Transparent;
				img.Clicked += (object sender, EventArgs e) =>
				{
					if (img.Image == "downarrow.png")
					{
						img.Image = "uparrow.png";
						foreach (View v in tablelayout.Children)
						{
							v.IsVisible = true;
						}
					}
					else
					{
						int i = 0;
						img.Image = "downarrow.png";
						foreach (View v in tablelayout.Children)
						{
							if (i == 0) { }
							else
							{
								v.IsVisible = false;
							}
							i++;
						}
					}
				};

				rowlayout.Children.Add(img);




				tablelayout.Children.Add(rowlayout);


				return tablelayout;

			}
			public StackLayout createseperatorbetweentables()
			{

				StackLayout line = new StackLayout();
				line.Orientation = StackOrientation.Horizontal;
				line.HeightRequest = 5;
				line.BackgroundColor = Color.LightGray;
				line.HorizontalOptions = LayoutOptions.Fill;
				return line;

			}
		public StackLayout createsupplierrowintable(string venuname,string pricep,string paidprice)
		{
			StackLayout vlayout = new StackLayout();
			vlayout.Orientation = StackOrientation.Vertical;
			vlayout.IsVisible = false;

			StackLayout line = new StackLayout();
			line.Orientation = StackOrientation.Horizontal;
			line.HeightRequest = 1;
			line.BackgroundColor = Color.LightGray;
			line.HorizontalOptions = LayoutOptions.Fill;
			vlayout.Children.Add(line);

			Label nameg = new Label();
			nameg.Text = venuname;
			nameg.FontSize = 16;
			nameg.TextColor = Color.Black;
			nameg.FontAttributes = FontAttributes.Bold;
			nameg.HorizontalOptions = LayoutOptions.FillAndExpand;
			nameg.Margin = new Thickness(15, 0, 0, 0);
			vlayout.Children.Add(nameg);


			StackLayout rowlayout = new StackLayout();
			rowlayout.Orientation = StackOrientation.Horizontal;
			rowlayout.Margin = new Thickness(15, 0, 15, 0);

			Label cost = new Label();
			cost.Text = "Cost:$ ";
			cost.FontSize = 12;
			cost.TextColor = Color.Black;
			//cost.Margin = new Thickness(15, 0, 0, 0);
			rowlayout.Children.Add(cost);

			Label price= new Label();
			price.Text = pricep;
			price.FontSize = 12;
			price.TextColor = Color.Black;
			//cost.HorizontalOptions = LayoutOptions.FillAndExpand;
			rowlayout.Children.Add(price);

			Label estimated = new Label();
			estimated.Text = "(estimate)";
			estimated.FontSize = 12;
			estimated.TextColor = Color.Black;
			estimated.HorizontalOptions = LayoutOptions.FillAndExpand;
			rowlayout.Children.Add(estimated);

			Label paid = new Label();
			paid.Text = "Paid:$ ";
			paid.FontSize = 12;
			paid.TextColor = Color.Black;
			//cost.Margin = new Thickness(15, 0, 0, 0);
			rowlayout.Children.Add(paid);

			Label paidp = new Label();
			paidp.Text = paidprice;
			paidp.FontSize = 12;
			paidp.TextColor = Color.Black;
			//cost.Margin = new Thickness(15, 0, 0, 0);
			rowlayout.Children.Add(paidp);







			vlayout.Children.Add(rowlayout);

			StackLayout line2 = new StackLayout();
			line2.Orientation = StackOrientation.Horizontal;
			line2.HeightRequest = 1;
			line2.BackgroundColor = Color.LightGray;
			line2.HorizontalOptions = LayoutOptions.Fill;
			vlayout.Children.Add(line);
			return vlayout;
		}

	}
	}
