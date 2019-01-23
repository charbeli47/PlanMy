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
        public newguest (bool isedit,guest guestt)
		{
			InitializeComponent ();
			backarrow.Clicked += async (object sender, EventArgs e) =>
			{
				await Navigation.PopModalAsync();
			};
			if (isedit == true && guestt != null)

			{
				deleteguestbut.IsVisible = true;
				guestname.Text = guestt.guest_name;
				guestphone.Text = guestt.Phone;
				guestemail.Text = guestt.Email;
				guestaddress.Text = guestt.Address;

				List<string> rspstatus = new List<string>();
				rspstatus.Add("Accepted");
				rspstatus.Add("Declined");
				rspstatus.Add("No Response");
				rspstatus.Add("Not Invited");
				int indexstatus = 0;
				int i = 0;
				foreach (string s in rspstatus)
				{
					if (guestt.RSVP == s)
					{
						indexstatus = i;
					}
					i++;
				}
				RspPicker.SelectedIndex = indexstatus;

				List<string> sides = new List<string>();
				sides.Add("Bridesmaids");
					sides.Add("Brides Friends");
					sides.Add(" Brides Family");
					sides.Add("Brides Family Friends");
					sides.Add("  Brides Coworkers ");
					sides.Add("	 Groomsmen ");
					sides.Add("Grooms Friends ");
					sides.Add("Grooms Family ");
					sides.Add("Grooms Family Friends");
					sides.Add("Grooms Coworkers");
					sides.Add("Bride an Grooom Friends");
					sides.Add(" Partner 1");
					sides.Add("Partner 2");
					sides.Add("Bridesmaids");

														  
																				
 int indexstatus1 = 0;
				int i1 = 0;
				foreach (string s in sides)
				{
					if (guestt.side == s)
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
            using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
			new KeyValuePair<string,string>("guest_name",guestname.Text),
			new KeyValuePair<string, string>("side",SidePicker.SelectedItem.ToString()),
				new KeyValuePair<string,string>("Address",guestaddress.Text),
			new KeyValuePair<string, string>("City",""),
			new KeyValuePair<string,string>("Phone",guestphone.Text),
			new KeyValuePair<string, string>("Email",guestemail.Text),
			new KeyValuePair<string, string>("RSVP",RspPicker.SelectedItem.ToString()),
			new KeyValuePair<string, string>("userid",usr.user.id.ToString())
		});

				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/guestlist.php?action=insert", formcontent);
				request.EnsureSuccessStatusCode();
				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

			}
		}
        public async Task<UserCookie> GetUser()
        {
            Connect con = new Connect();
            var usr = await con.GetData("User");
            UserCookie cookie = new UserCookie();
            if (!string.IsNullOrEmpty(usr))
            {
                cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(usr);
            }
            return cookie;
        }
        //function to edit task//
        public async void editguest(guest guestt)
		{
			
			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string,string>("list_id",guestt.list_id),
		new KeyValuePair<string,string>("guest_name",guestname.Text),
			new KeyValuePair<string, string>("side",SidePicker.SelectedItem.ToString()),
				new KeyValuePair<string,string>("Address",guestaddress.Text),
			new KeyValuePair<string, string>("City",""),
			new KeyValuePair<string,string>("Phone",guestphone.Text),
			new KeyValuePair<string, string>("Email",guestemail.Text),
			new KeyValuePair<string, string>("RSVP",RspPicker.SelectedItem.ToString())
			

		});


				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/guestlist.php?action=update", formcontent);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

			}
		}

		public async void deleteguest(guest guestt)
		{

			using (var cl = new HttpClient())
			{
				var formcontent = new FormUrlEncodedContent(new[]
				{
					new KeyValuePair<string,string>("list_id",guestt.list_id),
	


		});


				var request = await cl.PostAsync("https://planmy.me/maizonpub-api/guestlist.php?action=delete", formcontent);

				request.EnsureSuccessStatusCode();

				var response = await request.Content.ReadAsStringAsync();
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();

			}
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