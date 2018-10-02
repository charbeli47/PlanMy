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
	public partial class SettingsView : ContentView
	{
        ProfilePage _profile;
		public SettingsView (ProfilePage profile)
		{
            _profile = profile;
			InitializeComponent ();
		}

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var p = (StackLayout)Parent;
            p.Children.Remove(this);
            _profile.settingsVisible = false;
        }
    }
}