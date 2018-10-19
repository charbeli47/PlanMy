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
	public partial class messages : ContentPage
	{
		public List<message> listmsgs;
		public messages()
		{
			InitializeComponent();
			search.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new Vendors());
			};

			favorites.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new favourites());
			};

			message.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PushModalAsync(new messages());
			};

			listmsgs = new List<message>();
			message msg = new message();
			msg.title = "Candid Image";
			msg.msg = "Hello thanks for ....";
			msg.time = "-23h";
			message msg2 = new message();
			msg2.title = "Candid Image";
			msg2.msg = "Hello thanks for ....";
			msg2.time = "-23h";
			listmsgs.Add(msg);
			listmsgs.Add(msg2);
			MessagesListView.ItemsSource = listmsgs;
		}



	}

	public class message
	{
		public string title;
		public string msg;
		public string time;


	}
}
