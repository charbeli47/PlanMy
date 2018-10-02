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
	public partial class SignUp : ContentPage
	{
		public SignUp()
		{
			InitializeComponent();
        }

        private async void SignInLink_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }
    }
}