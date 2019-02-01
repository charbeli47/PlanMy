using PlanMy.Library;
using SendBird;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignUp : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public SignUp()
		{
			InitializeComponent();
        }

        private async void SignInLink_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            string date = "";
            Connect con = new Connect();
            var token = await con.GetData("FirebaseToken");
            string link = Statics.apiLink + "Register";
            string resp = await con.DownloadData(link, "Username=" + NameEntry.Text + "&Email=" + EmailEntry.Text + "&Password=" + PasswordEntry.Text + "&Token=" + token + "&FirstName=" + FirstNameEntry.Text + "&LastName=" + LastNameEntry.Text);
            try
            {
                var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(resp);
                if (!string.IsNullOrEmpty(u.Id))
                {
                    SendBirdClient.Connect(u.Id, (User user, SendBirdException ev) =>
                    {
                        if (ev != null)
                        {
                        // Error
                        return;
                        }
                        SendBirdClient.UpdateCurrentUserInfo(u.FirstName + " " + u.LastName, user.ProfileUrl, (SendBirdException e1) =>
                        {
                            if (e1 != null)
                            {
                            // Error
                            return;
                            }
                        });
                        if (SendBirdClient.GetPendingPushToken() == null) return;

                    // For Android
                    SendBirdClient.RegisterFCMPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) =>
                        {
                            if (e1 != null)
                            {
                            // Error.
                            return;
                            }

                            if (status == SendBirdClient.PushTokenRegistrationStatus.PENDING)
                            {
                            // Try registration after connection is established.
                        }
                        });

                    // For iOS
                    SendBirdClient.RegisterAPNSPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) =>
                        {
                            if (e1 != null)
                            {
                            // Error.
                            return;
                            }

                            if (status == SendBirdClient.PushTokenRegistrationStatus.PENDING)
                            {
                            // Try registration after connection is established.
                        }
                        });
                    });
                    await con.SaveData("User", resp);
                    OperationCompleted?.Invoke(this, EventArgs.Empty);
                    await Navigation.PopModalAsync();
                }
                else
                {
                    var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityResult>(resp);
                    DisplayAlert("Error", result.Errors.FirstOrDefault().Description, "OK");
                }
            }
            catch
            {
                var result = Newtonsoft.Json.JsonConvert.DeserializeObject<IdentityResult>(resp);
                DisplayAlert("Error", "An error occured, please try again later", "OK");
            }
            
        }
    }
}