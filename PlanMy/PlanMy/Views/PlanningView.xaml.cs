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
	public partial class PlanningView : ContentView
	{
		public PlanningView ()
		{
			InitializeComponent ();
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