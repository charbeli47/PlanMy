using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Facebook;
using PlanMy.Library;
using Android.Content;
using System.Threading.Tasks;
using System.IO;
using Xamd.ImageCarousel.Forms.Plugin.Droid;
using AuditApp.Android;
using CarouselView.FormsPlugin.Android;
using Plugin.Permissions;
using Firebase.Analytics;
using Firebase.Messaging;

namespace PlanMy.Droid
{
    [Activity(Label = "PlanMy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static readonly string[] PERMISSIONS = new[] { "publish_actions" };
        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }
        public static ICallbackManager CallbackManager;
        private FirebaseAnalytics firebaseAnalytics;
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;
			ImageCarouselRenderer.Init();
            CarouselViewRenderer.Init();
            Xamarin.FormsMaps.Init(this,bundle);
            AndroidPlaystoreAudit.Instance.UsesUntilPrompt = 6;
            AndroidPlaystoreAudit.Instance.TimeUntilPrompt = new TimeSpan(0, 0, 0, 0);
            AndroidPlaystoreAudit.Instance.Run(this);
            
            CallbackManager = CallbackManagerFactory.Create();
            
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            firebaseAnalytics = FirebaseAnalytics.GetInstance(this);
#if DEBUG

            try
            {
                Firebase.FirebaseApp.InitializeApp(this);
                //var instanceID = FirebaseInstanceId.Instance;
                //instanceID.DeleteInstanceId();
                // var iid1 = instanceID.Token;
                //var iid2 = instanceID.GetToken("1045913729593", Firebase.Messaging.FirebaseMessaging.InstanceIdScope);
                FirebaseMessaging.Instance.SubscribeToTopic("all");
            }
            catch (Exception ex)
            {

            }

#else
            Firebase.FirebaseApp.InitializeApp(this);
			FirebaseMessaging.Instance.SubscribeToTopic("all");
#endif
            LoadApplication(new App());
        }

        
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Permission[] grantResults)
        {
            PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
        
        protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
        {
            base.OnActivityResult(requestCode, resultCode, data);
            CallbackManager.OnActivityResult(requestCode, (int)resultCode, data);
            if (requestCode == PickImageId)
            {
                if ((resultCode == Result.Ok) && (data != null))
                {
                    Android.Net.Uri uri = data.Data;
                    Stream stream = ContentResolver.OpenInputStream(uri);

                    // Set the Stream as the completion of the Task
                    PickImageTaskCompletionSource.SetResult(stream);
                }
                else
                {
                    PickImageTaskCompletionSource.SetResult(null);
                }
            }
        }
    }
}

