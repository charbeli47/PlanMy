using ImageCircle.Forms.Plugin.Abstractions;
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
	public partial class ListEvents : ContentPage
	{
		public ListEvents (string q)
		{
			InitializeComponent ();
            NavigationPage.SetHasNavigationBar(this, false);
            LoadPage(q);
        }

        private async void LoadPage(string q)
        {
            Connect con = new Connect();
            var datas = await con.DownloadData(Statics.apiLink+"Events", "q=" + q);
            var events = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Events>>(datas);
            EventsListView.ItemsSource = events;
        }
        

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopAsync();
        }

        private void EventsListView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var evento = (Events)e.SelectedItem; ;
            Navigation.PushModalAsync(new ImageViewer(evento));
        }
    }
    public class EventObj
    {
        public string event_name { get; set; }
        public string event_date { get; set; }
        public string event_location { get; set; }
        public string event_img { get; set; }
    }
}