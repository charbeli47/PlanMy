using Plugin.ContactService.Shared;
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
    public partial class ContactList : ContentPage
    {
        public Contact contact = new Contact();
        public event EventHandler<EventArgs> OperationCompleted;
        public ContactList()
        {
            InitializeComponent();
            GetContacs();
        }
        async Task GetContacs()
        {
            var contacts = await Plugin.ContactService.CrossContactService.Current.GetContactListAsync();
            lstContacts.BindingContext = contacts;
        }
        private void lstContacts_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            contact = (Contact)e.SelectedItem;
            
            ((ListView)sender).SelectedItem = null;
            if (contact == null)
                return;
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            Navigation.PopModalAsync();
        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}