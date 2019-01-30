using PlanMy.Library;
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
		public ImageViewer (Events evento)
		{
            InitializeComponent ();
            if(EventImg!=null)
                EventImg.Source = evento.Image;
            if(Pagetitle!=null)
                Pagetitle.Text = evento.Title;
            if(EventLocation!=null)
                EventLocation.Text = "Location: " + evento.Description;
            if(EventDate!=null)
                EventDate.Text = "Date: " + evento.Date;
        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}