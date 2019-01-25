using PlanMy.Library;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class DealsPage : ContentPage
	{
		public DealsPage()
		{
			InitializeComponent();
            // go to pages on click//
            NavigationPage.SetHasNavigationBar(this, false);
            dealsbut.Clicked += (object sender, EventArgs e) =>
			{
                //DealsView.IsVisible = true;
               // BundlesView.IsVisible = false;
                dealsbut.Image = "hotdeals.png";
                bundlesbut.Image = "bundles.png";
                carouselView.Position = 0;
                
            };
			bundlesbut.Clicked += async(object sender, EventArgs e) =>
			{

               // DealsView.IsVisible = false;
                //BundlesView.IsVisible = true;
                dealsbut.Image = "hotdeals2.png";
                bundlesbut.Image = "bundles2.png";
                carouselView.Position = 1;
            };
            List<Views> vs = new List<Views>();
            DealsView deals = new DealsView();
            BundlesView bundles = new BundlesView();
            
            vs.Add(new Views { content = deals });
            vs.Add(new Views { content = bundles });
            carouselView.ItemsSource = vs;
            //DealsView.PropertyChanged += DealsView_PropertyChanged;
		}
        
        
        
        
        
        

        

        private void basketbut_Clicked(object sender, EventArgs e)
        {
            Navigation.PushModalAsync(new BasketView());
        }

        private void CategoriePicker_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }
        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ContentView.IsVisibleProperty.PropertyName)
                ForceLayout();
        }

        private void carouselView_PositionSelected(object sender, CarouselView.FormsPlugin.Abstractions.PositionSelectedEventArgs e)
        {
            switch(e.NewValue)
            {
                case 0:
                    dealsbut.Image = "hotdeals.png";
                    bundlesbut.Image = "bundles.png";
                    break;
                case 1:
                    dealsbut.Image = "hotdeals2.png";
                    bundlesbut.Image = "bundles2.png";
                    break;
            }
        }
    }

    // for testing purposes//
    public class deals{
        public int? id { get; set; }
		public string img { get; set; }
		public string title { get; set; }
        public Offers product { get; set; }
		public string desc { get; set; }
	}
    public class Views
    {
        public View content { get; set; }
    }
}