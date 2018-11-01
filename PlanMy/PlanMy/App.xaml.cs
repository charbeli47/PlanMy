using System;
using Xamarin.Forms;
using PlanMy.Views;
using Xamarin.Forms.Xaml;
using PlanMy.ViewModels;

[assembly: XamlCompilation (XamlCompilationOptions.Compile)]
namespace PlanMy
{
	public partial class App : Application
	{
		
		public App ()
		{
			InitializeComponent();


			MainPage = new MainPage();
		}
        public static Action<string> PostSuccessFacebookAction { get; set; }
        protected override void OnStart ()
		{
			// Handle when your app starts
		}

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
    public static class ViewModelLocator
    {
        static MainChatViewModel chatVM;
        public static MainChatViewModel MainChatViewModel
        {
            get
            {
                if (chatVM == null)
                {
                    chatVM = new MainChatViewModel(new VendorItem());
                }
                return chatVM;

            }
        }

    }
}
