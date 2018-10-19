using System;
using Android.App;
using Android.Content;
using Xamarin.Facebook;
using Xamarin.Facebook.Login;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlanMy;
using PlanMy.Library;
using PlanMy.Droid;
using Object = Java.Lang.Object;
using View = Android.Views.View;
using Xamarin.Facebook.Login.Widget;
using Xamarin.Facebook.Share;

[assembly: ExportRenderer(typeof(FacebookLoginButton), typeof(FacebookLoginButtonRendererAndroid))]
namespace PlanMy.Droid
{
    public class FacebookLoginButtonRendererAndroid : ViewRenderer<Button, LoginButton>
    {
        private static Activity _activity;

        protected override void OnElementChanged(ElementChangedEventArgs<Button> e)
        {

            base.OnElementChanged(e);

            _activity = this.Context as MainActivity;
            var loginButton = new LoginButton(Context);
            var facebookCallback = new FacebookCallback<LoginResult>
            {
                HandleSuccess = shareResult => {
                    App.PostSuccessFacebookAction?.Invoke(shareResult.AccessToken.Token);
                }
        ,
                HandleCancel = () => {
                    Console.WriteLine("HelloFacebook: Canceled");
                },
                HandleError = shareError => {
                    Console.WriteLine("HelloFacebook: Error: {0}", shareError);
                }
            };

            loginButton.RegisterCallback(MainActivity.CallbackManager, facebookCallback);
            //loginButton.SetBackgroundColor(new Android.Graphics.Color(255, 255, 255, 0));
            //loginButton.Text = "";
            //loginButton.SetWidth(44);
            //loginButton.SetHeight(43);
            //loginButton.SetTextColor(new Android.Graphics.Color(59, 89, 151));
           // loginButton.SetBackgroundResource(Resource.Drawable.fb);
            base.SetNativeControl(loginButton);
        }
    }
}