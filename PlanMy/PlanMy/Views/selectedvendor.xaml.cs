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
			//foreach (var itemm in selectedpost)
			//{
				//postimage.Source = itemm.Embedded.WpFeaturedmedia.ToList()[0].SourceUrl.Replace("https://", "http://");
			//}
		}
		
		}
	
}