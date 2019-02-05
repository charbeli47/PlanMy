using PlanMy.Library;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using TabbedPage = Xamarin.Forms.TabbedPage;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{

		public MainPage ()
		{
			InitializeComponent ();
            On<Xamarin.Forms.PlatformConfiguration.Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            LoadPage();
		}
        public async void LoadPage()
        {
            Connect con = new Connect();
            var user = await con.GetData("User");
            Login login = new Login();
            login.OperationCompleted += Login_OperationCompleted;
            if(string.IsNullOrEmpty(user))
                await Navigation.PushModalAsync(login);
        }

        private void Login_OperationCompleted(object sender, EventArgs e)
        {
            this.Children.Clear();
            Xamarin.Forms.NavigationPage profile = new Xamarin.Forms.NavigationPage(new ProfilePage());
            profile.Title = "Profile";
            profile.Icon = "profile.png";
            this.Children.Add(profile);
            Xamarin.Forms.NavigationPage vendors = new Xamarin.Forms.NavigationPage(new Vendors());
            vendors.Title = "Vendors";
            vendors.Icon = "vendors.png";
            this.Children.Add(vendors);
            Xamarin.Forms.NavigationPage planning = new Xamarin.Forms.NavigationPage(new Planning());
            planning.Title = "Planning";
            planning.Icon = "planning.png";
            this.Children.Add(planning);
            Xamarin.Forms.NavigationPage deals = new Xamarin.Forms.NavigationPage(new DealsPage());
            deals.Title = "Deals";
            deals.Icon = "deals.png";
            this.Children.Add(deals);
            Xamarin.Forms.NavigationPage ideas = new Xamarin.Forms.NavigationPage(new IdeasPage());
            ideas.Title = "Ideas";
            ideas.Icon = "ideas.png";
            this.Children.Add(ideas);
        }
    }
}