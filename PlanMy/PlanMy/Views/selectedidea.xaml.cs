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

			var browser = new WebView();
			var htmlSource = new HtmlWebViewSource();
			htmlSource.Html = html; 
			browser.Source = htmlSource;
		}
	}
}