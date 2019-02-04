using PlanMy.Library;
using PlanMy.ViewModels;
using SendBird;
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
	public partial class MainChatPage : ContentPage
	{
        MainChatViewModel vm;
        public MainChatPage(string vendorId, string thumb)
        {
            InitializeComponent();
			//Pagetitle.Text = "Chatting with " + vendor.User.FirstName;

			
			LoadPage(vendorId, thumb); 
            

        }
        void LoadPage(string vendor, string thumb)
        {
            Connect con = new Connect();
            BindingContext = vm = new MainChatViewModel(vendor, thumb);


            vm.Messages.CollectionChanged += (sender, e) =>
            {
                var target = vm.Messages[vm.Messages.Count - 1];
                MessagesListView.ScrollTo(target, ScrollToPosition.End, true);
            };
        }
        void MyListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            MessagesListView.SelectedItem = null;
        }

        void MyListView_OnItemTapped(object sender, ItemTappedEventArgs e)
        {
            MessagesListView.SelectedItem = null;

        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}