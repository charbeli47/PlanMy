using System;
using Android.App;
using Firebase.Iid;
using System.Net;
using PlanMy.Library;
using SendBird;
using SendBird.SBJson;
using Firebase.Messaging;
using Android.Content;
using Android.Support.V4.App;

namespace PlanMy.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.INSTANCE_ID_EVENT" })]
	public class FirebaseInstanceIdServiceImpl : FirebaseInstanceIdService
	{
        public const string URGENT_CHANNEL = "com.maizonpub.planmy.urgent";
        public async override void OnTokenRefresh()
		{
			// Get updated InstanceID token.
			var refreshedToken = FirebaseInstanceId.Instance.Token;
            
            Connect con = new Connect();
            var usr = await con.GetData("User");
            if (!string.IsNullOrEmpty(usr))
            {
                Users cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<Users>(usr);
                SendBirdClient.ChannelHandler ch = new SendBirdClient.ChannelHandler();
                SendBirdClient.Connect(cookie.Id, (User user, SendBirdException ev) =>
                {
                    if (ev != null)
                    {
                        // Error
                        return;
                    }
                    SendBird.SendBirdClient.RegisterFCMPushTokenForCurrentUser(refreshedToken, (SendBirdClient.PushTokenRegistrationStatus status, SendBirdException e1) =>
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
                    ch.OnMessageReceived = (BaseChannel baseChannel, BaseMessage baseMessage) => {
                        UserMessage userMsg = (UserMessage)baseMessage;
                        string chanName = "Chat Messages";
                        var importance = NotificationImportance.High;
                        NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, chanName, importance);
                        chan.EnableVibration(true);
                        chan.LockscreenVisibility = NotificationVisibility.Public;
                        sendNotification(userMsg.Sender.UserId, userMsg.Sender.Nickname, userMsg.Message, chan);
                        //Messages.Add(new Message { Text = userMsg.Message, IsIncoming = true, MessageDateTime = ConvertToDate(userMsg.CreatedAt), SenderImg = thumb });

                    };
                });
                
                WebClient client = new WebClient();
                var oldtoken = await con.GetData("FirebaseToken");
                string updateLink = Statics.apiLink + "AddPushToken?UserId=" + cookie.Id + "&NewToken=" + refreshedToken+"&OldToken="+oldtoken+"&PushDevice="+ PushDevice.Android;               
                client.DownloadString(updateLink);
            }
            await con.SaveData("FirebaseToken", refreshedToken);
        }
        private void sendNotification(string id, string username, string body, NotificationChannel chan)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            var notificationBuilder = new NotificationCompat.Builder(this)
                                                                            .SetSmallIcon(Resource.Drawable.logoplanmy)
                                                                            .SetContentTitle("New message from " + username)
                                                                            .SetContentText(body)
                                                                            .SetAutoCancel(true)
                                                                            .SetContentIntent(pendingIntent)
                                                                            .SetChannelId(URGENT_CHANNEL);
            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.CreateNotificationChannel(chan);
            notificationManager.Notify(1100, notificationBuilder.Build());
        }
    }
}
