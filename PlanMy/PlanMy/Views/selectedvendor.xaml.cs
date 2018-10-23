using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class selectedvendor : ContentPage
	{
		public selectedvendor(string catname, IEnumerable<WordPressPCL.Models.Post> selectedpost)
		{
			InitializeComponent();
			Pagetitle.Text = catname;
			backarrow.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};
		}
		
		}
	
}