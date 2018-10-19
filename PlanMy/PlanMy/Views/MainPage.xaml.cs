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
            On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
            LoadPage();
		}
        public async void LoadPage()
        {
            Connect con = new Connect();
            var user = await con.GetData("User");
            if(string.IsNullOrEmpty(user))
                await Navigation.PushModalAsync(new Login());
        }
	}
}