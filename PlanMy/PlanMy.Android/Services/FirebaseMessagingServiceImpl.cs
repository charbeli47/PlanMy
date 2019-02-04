using System;
using Android.App;
using Android.Content;
using Android.Support.V4.App;
using Firebase.Messaging;
using System.Net;
using Android.Graphics;
using Java.Net;
using PlanMy.ViewModels;
using PlanMy.Views;
using SendBird.SBJson;

namespace PlanMy.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class FirebaseMessagingServiceImpl : FirebaseMessagingService
	{
        // application is in the foreground, this method will fire.
        public const string URGENT_CHANNEL = "com.maizonpub.planmy.urgent";
        public override void OnMessageReceived(RemoteMessage message)
		{
            string chanName = "Chat Messages";
            var importance = NotificationImportance.High;
            NotificationChannel chan = new NotificationChannel(URGENT_CHANNEL, chanName, importance);
            chan.EnableVibration(true);
            chan.LockscreenVisibility = NotificationVisibility.Public;
            try
            {
                JsonElement payload = JsonParser.Parse(message.Data["sendbird"]);
                sendNotification(message, payload, chan);
            }
            catch
            {
                showNotification(message, chan);
            }
			
		}

        private void sendNotification(RemoteMessage message, JsonElement payload, NotificationChannel chan)
        {
            var intent = new Intent(this, typeof(MainActivity));
            intent.AddFlags(ActivityFlags.ClearTop);
            var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            string id = payload["sender"]["id"];
            string username = payload["sender"]["name"];
            string body = payload["message"];
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

        void showNotification(RemoteMessage message, NotificationChannel chan) 
		{
			var intent = new Intent(this, typeof(MainActivity));
			intent.AddFlags(ActivityFlags.ClearTop);
			var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.OneShot);
            string id = message.Data["key"];
            string imgUrl = "";
            
            //if (!string.IsNullOrEmpty(id))
            //{
            //    if (id == "1")
            //    {
            //        VendorItem vendorItem = new VendorItem { post_author = "1", featured_media = "chatlogo.png", post_title = "Plan My" };
            //        var mainPage = new MainChatPage(vendorItem);
            //        //pendingIntent..MainPage = mainPage;

            //    }
            //    else if (id != "0")
            //    {
            //        VendorItem vendorItem = new VendorItem { post_author = id, featured_media = "chatlogo.png", post_title = "Plan My Supplier" };
            //        var mainPage = new MainChatPage(vendorItem);
                    
            //        //_app.MainPage = mainPage;
            //    }
            //}

            if(!string.IsNullOrEmpty(imgUrl))
            {
                URL url = new URL(imgUrl);
                Android.Graphics.Bitmap mBitmap = BitmapFactory.DecodeStream(url.OpenConnection().InputStream);
                /*NotificationCompat.BigPictureStyle bigPictureStyle = new NotificationCompat.BigPictureStyle();
                bigPictureStyle.SetBigContentTitle(message.GetNotification().Title ?? "Fleurs De La Sagesse");
                bigPictureStyle.SetSummaryText(message.GetNotification().Body);
                var notificationBuilder = new NotificationCompat.Builder(this)
                    
                .SetLargeIcon(mBitmap)
                                                            .SetSmallIcon(Resource.Drawable.icon)
                                                            .SetContentTitle(message.GetNotification().Title ?? "Fleurs De La Sagesse")
                                                            .SetContentText(message.GetNotification().Body)
                                                            .SetAutoCancel(true)
                                                            .SetStyle(bigPictureStyle)
                                                            .SetContentIntent(pendingIntent);
                var notificationManager = NotificationManager.FromContext(this);

                notificationManager.Notify(0, notificationBuilder.Build());*/
                NotificationCompat.Builder mBuilder = new NotificationCompat.Builder(ApplicationContext);
                showBigNotification(mBitmap, mBuilder, message.GetNotification().Title ?? "Plan My", pendingIntent, message.GetNotification().Body, chan);
            }
            else
            {
                var notificationBuilder = new NotificationCompat.Builder(this)
                                                                            .SetSmallIcon(Resource.Drawable.logoplanmy)
                                                                            .SetContentTitle(message.GetNotification().Title ?? "Plan My")
                                                                            .SetContentText(message.GetNotification().Body)
                                                                            .SetAutoCancel(true)
                                                                            .SetContentIntent(pendingIntent)
                                                                            .SetChannelId(URGENT_CHANNEL);
                var notificationManager = NotificationManager.FromContext(this);
                notificationManager.CreateNotificationChannel(chan);
                notificationManager.Notify(1100, notificationBuilder.Build());
            }

			
		}
        private void showBigNotification(Bitmap bitmap, NotificationCompat.Builder mBuilder, String title, PendingIntent resultPendingIntent, string body, NotificationChannel chan)
        {
            NotificationCompat.BigPictureStyle bigPictureStyle = new NotificationCompat.BigPictureStyle();
            bigPictureStyle.SetBigContentTitle(title);
            bigPictureStyle.SetSummaryText(body);
            bigPictureStyle.BigPicture(bitmap);
            Notification notification;
           
            notification = mBuilder.SetSmallIcon(Resource.Drawable.logoplanmy).SetTicker(title).SetWhen(0)
                    .SetAutoCancel(true)
                    .SetContentTitle(title)
                    .SetContentIntent(resultPendingIntent)
                    .SetStyle(bigPictureStyle)
                    .SetSmallIcon(Resource.Drawable.logoplanmy)
                    .SetLargeIcon(bitmap)
                    .SetContentText(body)
                    .SetChannelId(URGENT_CHANNEL)
                    .Build();

            var notificationManager = NotificationManager.FromContext(this);
            notificationManager.CreateNotificationChannel(chan);
            notificationManager.Notify(1100, notification);
        }

    }
}
