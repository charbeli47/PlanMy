using Plugin.Messaging;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
		ObservableCollection<FileImageSource> imageSources = new ObservableCollection<FileImageSource>();
		public selectedvendor(string catname, WordPressPCL.Models.Item selectedpost)
		{
			InitializeComponent();
			Pagetitle.Text = catname;
			backarrow.Clicked += (object sender, EventArgs e) =>
			{
				Navigation.PopModalAsync();
			};


			Titlepost.Text=selectedpost.Title.Rendered;

			//image slider///
			imageSources.Add("vendor.png");
			imageSources.Add("vendor.png");
			imageSources.Add("vendor.png");


			imgSlider.Images = imageSources;

			seemap.GestureRecognizers.Add(new TapGestureRecognizer
			{
				Command = new Command(() => Navigation.PushModalAsync(new MapPage())),
			});

			// make phone call///
			phonebut.Clicked += (object sender, EventArgs e) =>
			{
				var PhoneCallTask = CrossMessaging.Current.PhoneDialer;
				if (PhoneCallTask.CanMakePhoneCall)
					PhoneCallTask.MakePhoneCall("+96170373528");
			};


		}




	}
	
}