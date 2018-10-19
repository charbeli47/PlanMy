﻿using PlanMy.Views;
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
	public partial class Planning : ContentPage
	{
		public Planning ()
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            planningView.TranslationX = 0;
            guestsView.TranslationX = Bounds.Width;
            budgetView.TranslationX = Bounds.Width * 2;
            checklistbut.Clicked += (object sender, EventArgs e) =>
			{
                planningView.IsVisible = true;
                guestsView.IsVisible = false;
                budgetView.IsVisible = false;
            };

			guestbut.Clicked += (object sender, EventArgs e) =>
			{
                guestsView = new guests();
                planningView.IsVisible = false;
                guestsView.IsVisible = true;
                budgetView.IsVisible = false;
                //await Navigation.PushAsync(new guests());
            };
			budgetbut.Clicked +=  (object sender, EventArgs e) =>
			{
                planningView.IsVisible = false;
                guestsView.IsVisible = false;
                budgetView.IsVisible = true;
            };
            suppliersbut.Clicked += async (s, e) => {
                await Navigation.PushAsync(new favourites());
            };
            StackLayout table1 = createtablerow("Venues", "location.png");
            StackLayout table2 = createtablerow("Lighting & sound", "location.png");
            table1.Children.Add(createsupplierrowintable("Domaine de Zekrit", "25 000", "10 000"));

            table2.Children.Add(createsupplierrowintable("Wicked Solutions", "5 000", "1000"));
            table2.Children.Add(createsupplierrowintable("Basement Music", "2 000", "200"));

            content.Children.Add(table1);
            content.Children.Add(createseperatorbetweentables());
            content.Children.Add(table2);
            content.Children.Add(createseperatorbetweentables());

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


            StackLayout gtable1 = createtableguestrow("The Hosts", "3");
            // add guest to table///
            gtable1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));
            gtable1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));
            gtable1.Children.Add(createguestrowintable("Rouzana Boustani", "attending.png"));


            seatcharc.Children.Add(gtable1);
            seatcharc.Children.Add(createseperatorbetweentables());
            seatcharc.Children.Add(createtableguestrow("The Haddads", "28"));
            seatcharc.Children.Add(createseperatorbetweentables());
            seatcharc.Children.Add(createtableguestrow("The Semaans", "30"));
            seatcharc.Children.Add(createseperatorbetweentables());
            newtask.Clicked += async (object sender, EventArgs e) =>
            {
                await Navigation.PushAsync(new newtask());
            };
            StackLayout december = createmonthstack("DECEMBER", "2018");
            december.Children.Add(donerow());
            december.Children.Add(donerow());
            december.Children.Add(seperatorbetweenmonths());

            StackLayout Feb = createmonthstack("February", "2019");
            Feb.Children.Add(priorityrowdone("Reserve your catering", "Catering & Bars"));
            Feb.Children.Add(notdonerow("Book a Dj", "Entertainment"));
            Feb.Children.Add(seperatorbetweenmonths());

            StackLayout March = createmonthstack("March", "2019");
            March.Children.Add(priorityrownotdone("Book my hairdresser", "Hair & Beuaty"));
            March.Children.Add(notdonerow("Book my make up artist", "Hair & Beuaty"));
            March.Children.Add(seperatorbetweenmonths());
            checkList.Children.Add(december);
            checkList.Children.Add(Feb);
            checkList.Children.Add(March);
            
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
            nametable.Margin = new Thickness(0, 10, 0, 0);
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
        public StackLayout createsupplierrowintable(string venuname, string pricep, string paidprice)
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

            Label price = new Label();
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
        public StackLayout createtableguestrow(string namet, string numberguests)
        {
            StackLayout tablelayout = new StackLayout();
            tablelayout.Orientation = StackOrientation.Vertical;

            StackLayout rowlayout = new StackLayout();
            rowlayout.Orientation = StackOrientation.Horizontal;
            rowlayout.Margin = new Thickness(15, 0, 15, 0);

            Image plusimg = new Image();
            plusimg.Source = "plus.png";
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

            Label numberg = new Label();
            numberg.Text = numberguests + " guests";
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

        public StackLayout createguestrowintable(string guestname, string status)
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
        public StackLayout createmonthstack(string month, string year)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Vertical;
            //stack.Margin = new Thickness(15, 0, 15, 0);
            Label title = new Label();
            title.Text = "BY " + month + " " + year;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 16;
            title.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(title);


            return stack;

        }
        public StackLayout priorityrownotdone(string titletxt, string descriptiontxt)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.GestureRecognizers.Add(
    new TapGestureRecognizer()
    {
        Command = new Command(() => { Navigation.PushModalAsync(new checklist()); })
    });
            Image imgcheck = new Image();
            imgcheck.Source = "notchecked.png";
            stack.Children.Add(imgcheck);
            imgcheck.Margin = new Thickness(10, 0, 0, 0);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
            title.Text = titletxt;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            StackLayout inhorizontalstack = new StackLayout();
            inhorizontalstack.Orientation = StackOrientation.Horizontal;

            inverticalstack.Children.Add(inhorizontalstack);

            Label priority = new Label();
            priority.Text = "Priority";
            priority.TextColor = Color.DarkRed;
            priority.FontAttributes = FontAttributes.Italic;
            priority.Margin = new Thickness(0, -5, 0, 0);
            priority.FontSize = 14;
            //desc.Margin = new Thickness(0, -5, 0, 0);

            inhorizontalstack.Children.Add(priority);

            Label desc = new Label();
            desc.Text = descriptiontxt;
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inhorizontalstack.Children.Add(desc);

            inverticalstack.HorizontalOptions = LayoutOptions.FillAndExpand;

            stack.Children.Add(inverticalstack);


            Image imgpriority = new Image();
            imgpriority.Source = "priority.png";
            imgpriority.HorizontalOptions = LayoutOptions.End;
            imgpriority.Margin = new Thickness(0, 0, 10, 0);

            stack.Children.Add(imgpriority);



            return stack;

        }
        public StackLayout priorityrowdone(string titletxt, string descriptiontxt)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.GestureRecognizers.Add(
    new TapGestureRecognizer()
    {
        Command = new Command(() => { Navigation.PushModalAsync(new checklist()); })
    });

            Image imgcheck = new Image();
            imgcheck.Source = "checked.png";
            imgcheck.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(imgcheck);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
            title.Text = titletxt;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            StackLayout inhorizontalstack = new StackLayout();
            inhorizontalstack.Orientation = StackOrientation.Horizontal;

            inverticalstack.Children.Add(inhorizontalstack);

            Label priority = new Label();
            priority.Text = "Priority";
            priority.TextColor = Color.DarkRed;
            priority.FontAttributes = FontAttributes.Italic;
            priority.Margin = new Thickness(0, -5, 0, 0);
            priority.FontSize = 14;
            //desc.Margin = new Thickness(0, -5, 0, 0);

            inhorizontalstack.Children.Add(priority);

            Label desc = new Label();
            desc.Text = descriptiontxt;
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inhorizontalstack.Children.Add(desc);

            inverticalstack.HorizontalOptions = LayoutOptions.FillAndExpand;

            stack.Children.Add(inverticalstack);


            Image imgpriority = new Image();
            imgpriority.Source = "priority.png";
            imgpriority.HorizontalOptions = LayoutOptions.End;
            imgpriority.Margin = new Thickness(0, 0, 10, 0);
            stack.Children.Add(imgpriority);



            return stack;

        }

        public StackLayout notdonerow(string titletxt, string descriptiontxt)
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.GestureRecognizers.Add(
    new TapGestureRecognizer()
    {
        Command = new Command(() => { Navigation.PushModalAsync(new checklist()); })
    });
            Image imgcheck = new Image();
            imgcheck.Source = "notchecked.png";
            imgcheck.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(imgcheck);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
            title.Text = titletxt;
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            Label desc = new Label();
            desc.Text = descriptiontxt;
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inverticalstack.Children.Add(desc);

            stack.Children.Add(inverticalstack);


            return stack;

        }
        public StackLayout donerow()
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.GestureRecognizers.Add(
    new TapGestureRecognizer()
    {
        Command = new Command(() => { Navigation.PushModalAsync(new checklist()); })
    });
            Image imgcheck = new Image();
            imgcheck.Source = "checked.png";
            imgcheck.Margin = new Thickness(10, 0, 0, 0);
            stack.Children.Add(imgcheck);

            StackLayout inverticalstack = new StackLayout();
            inverticalstack.Orientation = StackOrientation.Vertical;
            inverticalstack.Margin = new Thickness(15, 0, 0, 0);
            Label title = new Label();
            title.Text = "Schedule an engaement photoshoot";
            title.TextColor = Color.Black;
            title.FontAttributes = FontAttributes.Bold;
            title.FontSize = 14;
            inverticalstack.Children.Add(title);

            Label desc = new Label();
            desc.Text = "Photography and Videography";
            desc.TextColor = Color.Gray;
            desc.FontAttributes = FontAttributes.Italic;
            desc.FontSize = 14;
            desc.Margin = new Thickness(0, -5, 0, 0);
            inverticalstack.Children.Add(desc);

            stack.Children.Add(inverticalstack);


            return stack;

        }

        public StackLayout seperatorbetweenmonths()
        {
            StackLayout stack = new StackLayout();
            stack.Orientation = StackOrientation.Horizontal;
            stack.HeightRequest = 1;
            stack.BackgroundColor = Color.Black;
            return stack;

        }
    }
}