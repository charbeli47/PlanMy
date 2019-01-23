using System;
using System.Collections.Generic;
using System.Linq;
using CarouselView.FormsPlugin.iOS;
using Facebook.CoreKit;
using Foundation;
using MTiRate;
using UIKit;
using Xamd.ImageCarousel.Forms.Plugin.iOS;
using Firebase.CloudMessaging;
using UserNotifications;
using System.Net;
using PlanMy.Library;
using PlanMy.Views;
using PlanMy.ViewModels;

namespace PlanMy.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate, IUNUserNotificationCenterDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public static readonly string[] PERMISSIONS = new[] { "publish_actions" };
        protected int senderId = 0;
        protected App _app;
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();
            
            iRate.SharedInstance.DaysUntilPrompt = 0;
            iRate.SharedInstance.UsesUntilPrompt = 6;
            Xamarin.FormsMaps.Init();
            _app = new App();
			LoadApplication(_app);
            UINavigationBar.Appearance.TintColor = UIColor.FromRGB(0, 184, 192);
            ImageCarouselRenderer.Init();
            CarouselViewRenderer.Init();
            if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
            {
                // iOS 10
                var authOptions = UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound;
                UNUserNotificationCenter.Current.RequestAuthorization(authOptions, (granted, error) =>
                {
                    Console.WriteLine(granted);
                });

                // For iOS 10 display notification (sent via APNS)
                UNUserNotificationCenter.Current.Delegate = this;
            }
            else
            {
                // iOS 9 <=
                var allNotificationTypes = UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound;
                var settings = UIUserNotificationSettings.GetSettingsForTypes(allNotificationTypes, null);
                UIApplication.SharedApplication.RegisterUserNotificationSettings(settings);
            }

            UIApplication.SharedApplication.RegisterForRemoteNotifications();
            // Firebase component initialize
            //Firebase.Analytics.Analytics.Configure();

            Firebase.InstanceID.InstanceId.Notifications.ObserveTokenRefresh(async (sender, e) =>
            {
                var newToken = Firebase.InstanceID.InstanceId.SharedInstance.Token;
                WebClient client = new WebClient();
                Connect con = new Connect();
                string user = await con.GetData("User");
                if (user != "")
                {
                    UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(user);
                    int userId = cookie.user.id;
                    client.DownloadString("https://planmy.me/maizonpub-api/add_device.php?action=insertdevice&token=" + newToken + "&device=iOS&userid=" + userId);
                }
                else
                {
                    client.DownloadString("https://planmy.me/maizonpub-api/add_device.php?action=insertdevice&token=" + newToken + "&device=iOS");
                }
                // if you want to send notification per user, use this token
                System.Diagnostics.Debug.WriteLine(newToken);

                connectFCM();
            });
            return base.FinishedLaunching(app, options);
        }
        public override bool OpenUrl(UIApplication application, NSUrl url, string sourceApplication, NSObject annotation)
        {
            // We need to handle URLs by passing them to their own OpenUrl in order to make the SSO authentication works.
            return ApplicationDelegate.SharedInstance.OpenUrl(application, url, sourceApplication, annotation);
        }
        public override void DidEnterBackground(UIApplication uiApplication)
        {
            Messaging.SharedInstance.Disconnect();
        }
        public override void OnActivated(UIApplication uiApplication)
        {
            connectFCM();
            //base.OnActivated(uiApplication);
        }
        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }



        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }
        public override void RegisteredForRemoteNotifications(UIApplication application, NSData deviceToken)
        {
            //base.RegisteredForRemoteNotifications(application, deviceToken);
            var token = Firebase.InstanceID.InstanceId.SharedInstance.Token;

            SaveToken(token);


        }

        private async void SaveToken(string token)
        {
            Connect con = new Connect();
            await con.SaveData("FirebaseToken", token);
        }

        public override void ReceivedRemoteNotification(UIApplication application, NSDictionary userInfo)
        {
            // If you are receiving a notification message while your app is in the background,
            // this callback will not be fired till the user taps on the notification launching the application.
            // TODO: Handle data of notification

            // With swizzling disabled you must let Messaging know about the message, for Analytics
            //Messaging.SharedInstance.AppDidReceiveMessage (userInfo);

            // Print full message.
            Console.WriteLine(userInfo);
        }
        // iOS 9 <=, fire when recieve notification foreground
        public async override void DidReceiveRemoteNotification(UIApplication application, NSDictionary userInfo, Action<UIBackgroundFetchResult> completionHandler)
        {
            Messaging.SharedInstance.AppDidReceiveMessage(userInfo);

            // Generate custom event
            NSString[] keys = { new NSString("Event_type") };
            NSObject[] values = { new NSString("Recieve_Notification") };
            var parameters = NSDictionary<NSString, NSObject>.FromObjectsAndKeys(keys, values, keys.Length);

            // Send custom event
            Firebase.Analytics.Analytics.LogEvent("CustomEvent", parameters);
            if (userInfo["key"] != null)
            {
                string id = userInfo["key"] as NSString;
                senderId = int.Parse(id);
            }
            if (application.ApplicationState == UIApplicationState.Active)
            {
                System.Diagnostics.Debug.WriteLine(userInfo);
                var aps_d = userInfo["aps"] as NSDictionary;
                var alert_d = aps_d["alert"] as NSDictionary;
                var body = alert_d["body"] as NSString;
                var title = alert_d["title"] as NSString;
                debugAlert(title, body);
            }
            else
            {
                Connect con = new Connect();
                UIStoryboard storyBoard = UIStoryboard.FromName("Main", null);
                var user = await con.GetData("User");
                if (user != "")
                {
                    UserCookie cookie = Newtonsoft.Json.JsonConvert.DeserializeObject<UserCookie>(user);
                    int userId = cookie.user.id;
                    if (senderId == 1)
                    {
                        VendorItem vendorItem = new VendorItem { post_author = "1", featured_media = "chatlogo.png", post_title = "Plan My" };
                        var mainPage = new MainChatPage(vendorItem);

                        _app.MainPage = mainPage;
                        
                    }
                    else if (senderId != 0)
                    {
                        VendorItem vendorItem = new VendorItem { post_author = senderId.ToString(), featured_media = "chatlogo.png", post_title = "Plan My Supplier" };
                        var mainPage = new MainChatPage(vendorItem);
                        MainPage indexpage = (MainPage)_app.MainPage;
                        await indexpage.Navigation.PushModalAsync(mainPage);
                    }
                }
            }

        }

        // iOS 10, fire when recieve notification foreground
        /* [Export("userNotificationCenter:willPresentNotification:withCompletionHandler:")]
         public void WillPresentNotification(UNUserNotificationCenter center, UNNotification notification, Action<UNNotificationPresentationOptions> completionHandler)
         {
             UNNotificationRequest Request = notification.Request;
             UNNotificationContent content = Request.Content;

             var title = content.Title;
             var body = content.Body;
             debugAlert(title, body);
         }*/

        private void connectFCM()
        {
            try
            {
                Firebase.Core.App.Configure();
                Messaging.SharedInstance.ShouldEstablishDirectChannel = true;
            }
            catch
            {

            }
            Messaging.SharedInstance.Connect((error) =>
            {
                if (error == null)
                {
                    Messaging.SharedInstance.Subscribe("/topics/general");
                }
                System.Diagnostics.Debug.WriteLine(error != null ? "error occured" : "connect success");
            });
        }

        private void debugAlert(string title, string message)
        {
            var alert = new UIAlertView(title ?? "Title", message ?? "Message", null, "Cancel", "OK");
            alert.Show();
            alert.Clicked += Alert_Clicked;
        }

        private async void Alert_Clicked(object sender, UIButtonEventArgs e)
        {
            if (e.ButtonIndex == 1)
            {

                Connect con = new Connect();
                
                var user = await con.GetData("User");
                if (user != "")
                {

                    
                    if (senderId == 1)
                    {
                        VendorItem vendorItem = new VendorItem { post_author = "1", featured_media = "chatlogo.png", post_title = "Plan My" };
                        var mainPage = new MainChatPage(vendorItem);

                        _app.MainPage = mainPage;
                    }
                    else if (senderId != 0)
                    {
                        VendorItem vendorItem = new VendorItem { post_author = senderId.ToString(), featured_media = "chatlogo.png", post_title = "Plan My Supplier" };
                        var mainPage = new MainChatPage(vendorItem);

                        _app.MainPage = mainPage;
                    }
                }
            }
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }
    }
}
