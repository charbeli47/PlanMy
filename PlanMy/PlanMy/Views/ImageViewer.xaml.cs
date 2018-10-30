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
	public partial class ImageViewer : ContentPage
	{
		public ImageViewer (EventObj evento)
		{
            InitializeComponent ();
            if(EventImg!=null)
                EventImg.Source = evento.event_img;
            if(Pagetitle!=null)
                Pagetitle.Text = evento.event_name;
            if(EventLocation!=null)
                EventLocation.Text = "Location: " + evento.event_location;
            if(EventDate!=null)
                EventDate.Text = "Date: " + evento.event_date;
        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}