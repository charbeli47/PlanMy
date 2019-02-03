using Newtonsoft.Json;
using PlanMy.Library;
using PlanMy.Models;
using PlanMy.ViewModels;
using SendBird;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class Login : ContentPage
	{
        public event EventHandler<EventArgs> OperationCompleted;
        public Login ()
		{
			InitializeComponent ();
            App.PostSuccessFacebookAction = async token =>
            {
                var vm = BindingContext as FacebookViewModel;
                var requestUrl =
                "https://graph.facebook.com/v3.1/me/?fields=name,picture,work,website,religion,location,locale,link,cover,age_range,birthday,devices,email,first_name,last_name,gender,hometown,is_verified,languages,photos&access_token="
                + token;

                var httpClient = new HttpClient();

                var userJson = await httpClient.GetStringAsync(requestUrl);
                Connect con = new Connect();                
                var facebookProfile = JsonConvert.DeserializeObject<FacebookProfile>(userJson);
                requestUrl = "https://graph.facebook.com/me/picture?type=large&access_token=" + token;
                facebookProfile.Picture.Data.Url = requestUrl;
                await con.SaveData("FaceBookProfile", userJson);
                string password = CreatePassword(8);
                string link = Statics.apiLink + "Register?Username=" + facebookProfile.FirstName + "&Email=" + facebookProfile.Email + "&Password=" + password + "&FBToken=" + facebookProfile.Id;
                WebClient client = new WebClient();
                string resp = client.DownloadString(link);
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

                //you can use this token to authenticate to the server here
                //call your FacebookLoginService.LoginToServer(token)
                //I'll just navigate to a screen that displays the token:
                //await Navigation.PushAsync(new DiplayTokenPage(token));


            };
        }
        public string CreatePassword(int length)
        {
            const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
            StringBuilder res = new StringBuilder();
            Random rnd = new Random();
            while (0 < length--)
            {
                res.Append(valid[rnd.Next(valid.Length)]);
            }
            return res.ToString();
        }
        private async void SignUpLink_Clicked(object sender, EventArgs e)
        {
            var signUp = new SignUp();
            signUp.OperationCompleted += async (s, ev) => {
                OperationCompleted?.Invoke(this, EventArgs.Empty);
                await Navigation.PopModalAsync();
            };
            await Navigation.PushModalAsync(signUp);
        }

        private async void SkipBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void Button_Clicked(object sender, EventArgs e)
        {
            Connect con = new Connect();
            string link = Statics.apiLink + "Login";
            string resp = await con.DownloadData(link, "Username=" + UsernameEntry.Text + "&Password=" + PasswordEntry.Text + "&RememberMe=false");
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
                    con.DeleteData("FaceBookProfile");
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

        private async void fbBtn_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new FacebookProfilePage());
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new ForgotPassword());
        }
    }
}