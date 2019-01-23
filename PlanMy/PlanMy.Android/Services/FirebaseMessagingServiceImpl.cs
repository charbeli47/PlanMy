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

namespace PlanMy.Droid
{
	[Service]
	[IntentFilter(new[] { "com.google.firebase.MESSAGING_EVENT" })]
	public class FirebaseMessagingServiceImpl : FirebaseMessagingService
	{
		// application is in the foreground, this method will fire.
		public override void OnMessageReceived(RemoteMessage message)
		{
			System.Diagnostics.Debug.WriteLine(message.GetNotification().Body);
			showNotification(message);
		}

		void showNotification(RemoteMessage message) 
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
                showBigNotification(mBitmap, mBuilder, message.GetNotification().Title ?? "Plan My", pendingIntent, message.GetNotification().Body);
            }
            else
            {
                var notificationBuilder = new NotificationCompat.Builder(this)
                                                                            .SetSmallIcon(Resource.Drawable.logoplanmy)
                                                                            .SetContentTitle(message.GetNotification().Title ?? "Plan My")
                                                                            .SetContentText(message.GetNotification().Body)
                                                                            .SetAutoCancel(true)
                                                                            .SetContentIntent(pendingIntent);
                var notificationManager = NotificationManager.FromContext(this);

                notificationManager.Notify(0, notificationBuilder.Build());
            }

			
		}
        private void showBigNotification(Bitmap bitmap, NotificationCompat.Builder mBuilder, String title, PendingIntent resultPendingIntent, string body)
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
                    .Build();

            var notificationManager = NotificationManager.FromContext(this);

            notificationManager.Notify(0, notification);
        }

    }
}
