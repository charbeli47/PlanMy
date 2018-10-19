using PlanMy.Library;
using System;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class MainPage : TabbedPage
	{
		public MainPage ()
		{
			InitializeComponent ();
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