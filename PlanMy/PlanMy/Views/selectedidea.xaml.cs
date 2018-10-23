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
	public partial class selectedidea : ContentPage
	{
		public selectedidea (string html)
		{
			InitializeComponent ();
			NavigationPage.SetHasNavigationBar(this, false);

			backarrow.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};



			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = @html; 
			webv.Source = htmlSource;
		
		}
	}
}