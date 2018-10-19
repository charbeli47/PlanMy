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
	public partial class guests : ContentView
	{
		public guests ()
		{
			InitializeComponent ();
            addtablebut.Clicked += (object sender, EventArgs e) =>
			{
				popupaddtable.IsVisible = true;
			};

			addguestbut.Clicked += (object sender, EventArgs e) =>
			{
				popupguest.IsVisible = true;
			};
			closepopuptable.Clicked += (object sender, EventArgs e) =>
			{
				popupaddtable.IsVisible = false;
			};
			closepopupguest.Clicked += (object sender, EventArgs e) =>
			{
				popupguest.IsVisible = false;
			};

			



			allguest.Clicked += (object sender, EventArgs e) =>
			{
				allguest.Image = "ballguest.png";
				seatchart.Image = "seatingchart.png";
				allguestc.IsVisible = true;
				seatcharc.IsVisible = false;
			};
			seatchart.Clicked += (object sender, EventArgs e) =>
			{
				allguest.Image = "allguest.png";
				seatchart.Image = "bseatingchart.png";
				allguestc.IsVisible = false;
				seatcharc.IsVisible = true;
			};

			singleguest sg = new singleguest();
			sg.name = "Anthony Assaad";
			sg.status = "notattending.png";

			singleguest sg1 = new singleguest();
			sg1.name = "Marc Assaad";
			sg1.status = "attending.png";


			singleguest sg2 = new singleguest();
			sg2.name = "Marwan Maalouf";
			sg2.status = "pending2.png";
			List<singleguest> lg = new List<singleguest>();

			lg.Add(sg);
			lg.Add(sg1);
			lg.Add(sg2);

			guestList.ItemsSource = lg;


			StackLayout table1 = createtablerow("The Hosts", "3");
			// add guest to table///
			table1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));
			table1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));
			table1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));
			

			seatcharc.Children.Add(table1);
			seatcharc.Children.Add(createseperatorbetweentables());
			seatcharc.Children.Add(createtablerow("The Haddads", "28"));
			seatcharc.Children.Add(createseperatorbetweentables());
			seatcharc.Children.Add(createtablerow("The Semaans", "30"));
			seatcharc.Children.Add(createseperatorbetweentables());
		}

		public StackLayout createtablerow(string namet, string numberguests)
		{
			StackLayout tablelayout = new StackLayout();
			tablelayout.Orientation = StackOrientation.Vertical;

			StackLayout rowlayout = new StackLayout();
			rowlayout.Orientation = StackOrientation.Horizontal;
			rowlayout.Margin = new Thickness(15, 0, 15, 0);

			Image plusimg = new Image();
			plusimg.Source = "plus.png";
			plusimg.HorizontalOptions = LayoutOptions.Start;
			plusimg.Margin = new Thickness(0,0,25,0);
			plusimg.VerticalOptions = LayoutOptions.Center;
			rowlayout.Children.Add(plusimg);

			StackLayout vlayout = new StackLayout();
			vlayout.Orientation = StackOrientation.Vertical;

			Label nametable = new Label();
			nametable.Text=namet;
			nametable.FontSize = 18;
			nametable.TextColor = Color.Black;

			Label numberg = new Label();
			numberg.Text = numberguests+" guests";
			numberg.FontSize = 12;
			numberg.FontAttributes = FontAttributes.Italic;
			numberg.TextColor = Color.Black;
			numberg.Margin = new Thickness(0, -5, 0, 0);

			vlayout.Children.Add(nametable);
			vlayout.Children.Add(numberg);
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
					foreach(View v in tablelayout.Children)
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

		public StackLayout createguestrowintable(string guestname,string status)
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

			StackLayout rowlayout = new StackLayout();
			rowlayout.Orientation = StackOrientation.Horizontal;
			rowlayout.Margin = new Thickness(15, 0, 15, 0);

			Image plusimg = new Image();
			plusimg.Source = "guesttable.png";
			plusimg.HorizontalOptions = LayoutOptions.Start;
			plusimg.Margin = new Thickness(0, 0, 25, 0);
			plusimg.VerticalOptions = LayoutOptions.Center;
			rowlayout.Children.Add(plusimg);

			

			Label nameg = new Label();
			nameg.Text = guestname;
			nameg.FontSize = 16;
			nameg.TextColor = Color.Black;
			nameg.HorizontalOptions = LayoutOptions.FillAndExpand;
			rowlayout.Children.Add(nameg);

			Image img = new Image();
			img.Source = status;
			img.HorizontalOptions = LayoutOptions.End;
			img.VerticalOptions = LayoutOptions.Center;
			rowlayout.Children.Add(img);

			vlayout.Children.Add(rowlayout);

			StackLayout line2 = new StackLayout();
			line2.Orientation = StackOrientation.Horizontal;
			line2.HeightRequest = 1;
			line2.BackgroundColor = Color.LightGray;
			line2.HorizontalOptions = LayoutOptions.Fill;
			vlayout.Children.Add(line);
			return vlayout;
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

	}

	public class singleguest
	{
		public string name { get; set; }
		public string status { get; set; }
	}
}