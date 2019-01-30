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
            //string link = "http://weddexonline.com/maizonpub-api/users.php?action=register&username=" + NameEntry.Text + "&email=" + EmailEntry.Text + "&weddingdate=" + date + "&password=" + PasswordEntry.Text;
            var token = await con.GetData("FirebaseToken");
            string link = Statics.apiLink + "Register?Username="+NameEntry.Text+"&Email="+EmailEntry.Text+"&Password="+PasswordEntry.Text+"&Token="+token;
            WebClient client = new WebClient();
            string resp = client.DownloadString(link);
            var u = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(resp);
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
                SendBirdClient.RegisterFCMPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) => {
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
                SendBirdClient.RegisterAPNSPushTokenForCurrentUser(SendBirdClient.GetPendingPushToken(), (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) => {
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
    }
}