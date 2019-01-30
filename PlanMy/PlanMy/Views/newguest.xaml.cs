using Newtonsoft.Json;
using PlanMy.Library;
using Plugin.ContactService.Shared;
using Plugin.Permissions.Abstractions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class newguest : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public newguest (bool isedit,GuestList guestt)
		{
			InitializeComponent ();
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PopModalAsync();
			};
			if (isedit == true && guestt != null)

			{
				deleteguestbut.IsVisible = true;
				guestname.Text = guestt.FullName;
				guestphone.Text = guestt.Phone;
				guestemail.Text = guestt.Email;
				guestaddress.Text = guestt.Address;

				List<GuestStatus> rspstatus = new List<GuestStatus>();
				rspstatus.Add(GuestStatus.Accepted);
				rspstatus.Add(GuestStatus.Declined);
				rspstatus.Add(GuestStatus.No_Response);
				rspstatus.Add(GuestStatus.Not_Invited);
				int indexstatus = 0;
				int i = 0;
				foreach (GuestStatus s in rspstatus)
				{
					if (guestt.GuestStatus == s)
					{
						indexstatus = i;
					}
					i++;
				}
				RspPicker.SelectedIndex = indexstatus;

				List<Side> sides = new List<Side>();
				sides.Add(Side.Bridesmaids);
					sides.Add(Side.Brides_Friends);
					sides.Add(Side.Brides_Family);
					sides.Add(Side.Brides_Family_Friends);
					sides.Add(Side.Brides_Coworkers);
					sides.Add(Side.Groomsmen);
					sides.Add(Side.Grooms_Friends);
					sides.Add(Side.Grooms_Family);
					sides.Add(Side.Grooms_Family_Friends);
					sides.Add(Side.Grooms_Coworkers);
					sides.Add(Side.Bride_and_Groom_Friends);
					sides.Add(Side.Partner_1);
					sides.Add(Side.Partner_2);
					sides.Add(Side.Bridesmaids);

														  
																				
 int indexstatus1 = 0;
				int i1 = 0;
				foreach (Side s in sides)
				{
					if (guestt.Side == s)
					{
						indexstatus1 = i1;
					}
					i1++;
				}
				SidePicker.SelectedIndex = indexstatus1;
				//guestname.Text = guestt.guest_name;
				//guestname.Text = guestt.guest_name;
			}

			deleteguestbut.Clicked += async (object sender, EventArgs e) =>
			{
				deleteguest(guestt);

			};

			Savechanges.Clicked += async (object sender, EventArgs e) =>
			{
				if (isedit == true && guestt != null)
				{
					editguest(guestt);


				}
				else
				{
					addguest();
				}


			};
		}

		//function to add task//
		public async void addguest()
		{
            var usr = await GetUser();

            GuestList guest = new GuestList { Address = guestaddress.Text, City = "", Email = guestemail.Text, Phone = guestphone.Text, FullName = guestname.Text, GuestStatus = (GuestStatus)RspPicker.SelectedIndex, Side = (Side)SidePicker.SelectedIndex, UserId = usr.Id };
            string json = JsonConvert.SerializeObject(guest);
            Connect con = new Connect();
            con.PostToServer(Statics.apiLink + "GuestLists", json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}
        public async Task<Users> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            Users cookie = new Users();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
            }
            return cookie;
        }
        //function to edit task//
        public async void editguest(GuestList guestt)
		{
            Connect con = new Connect();
            var json = JsonConvert.SerializeObject(guestt);
            con.PutToServer(Statics.apiLink + "GuestLists/" + guestt.Id, json);
            OperationCompleted?.Invoke(this, EventArgs.Empty);
            await Navigation.PopModalAsync();
		}

		public async void deleteguest(GuestList guestt)
		{
            Connect con = new Connect();
            con.DeleteFromServer(Statics.apiLink + "GuestLists/" + guestt.Id);
			
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
		}

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var perm = await Utils.CheckPermissions(Permission.Contacts);
            if(perm ==PermissionStatus.Granted)
            {
                var ContactList = new ContactList();
                ContactList.OperationCompleted += (s,ev)=> {
                    var contact = ContactList.contact;
                    guestname.Text = contact.Name;
                    guestemail.Text = contact.Email;
                    guestphone.Text = contact.Number;
                };
                await Navigation.PushModalAsync(ContactList);


            }
        }
    }
}