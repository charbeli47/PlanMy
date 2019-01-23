using System;
using Android.App;
using Android.Content;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlanMy;
using PlanMy.LibFacebook;
using PlanMy.Droid;
using Object = Java.Lang.Object;
using View = Android.Views.View;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Share;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRenderer))]
namespace PlanMy.Droid
{
    public class FacebookLoginButtonRenderer : ViewRenderer
    {
        Context ctx;
        bool disposed;
        public FacebookLoginButtonRenderer(Context ctx) : base(ctx)
        {
            this.ctx = ctx;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
        {
            if (Control == null)
            {
                var fbLoginBtnView = e.NewElement as FacebookLoginButton;
                var fbLoginbBtnCtrl = new Xamarin.Facebook.Login.Widget.LoginButton(ctx)
                {
                    LoginBehavior = LoginBehavior.NativeWithFallback
                };

                fbLoginbBtnCtrl.SetReadPermissions(fbLoginBtnView.Permissions);
                fbLoginbBtnCtrl.RegisterCallback(MainActivity.CallbackManager, new MyFacebookCallback(this.Element as FacebookLoginButton));

                SetNativeControl(fbLoginbBtnCtrl);
            }
        }



        class MyFacebookCallback : Java.Lang.Object, IFacebookCallback
        {
            FacebookLoginButton view;

            public MyFacebookCallback(FacebookLoginButton view)
            {
                this.view = view;
            }

            public void OnCancel() =>
                view.OnCancel?.Execute(null);

            public void OnError(FacebookException fbException) =>
                view.OnError?.Execute(fbException.Message);

            public void OnSuccess(Java.Lang.Object result)
            {
                view.OnSuccess?.Execute(((LoginResult)result).AccessToken.Token);
                
            }
            
            

        }
    }
}