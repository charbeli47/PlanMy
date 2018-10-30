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

namespace PlanMy.Droid
{
    [Activity(Label = "PlanMy", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        public static ICallbackManager CallbackManager = CallbackManagerFactory.Create();
        public static readonly string[] PERMISSIONS = new[] { "publish_actions" };
        public static readonly int PickImageId = 1000;
        public TaskCompletionSource<Stream> PickImageTaskCompletionSource { set; get; }
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
            base.OnCreate(bundle);
            FacebookSdk.SdkInitialize(this.ApplicationContext);
            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
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

