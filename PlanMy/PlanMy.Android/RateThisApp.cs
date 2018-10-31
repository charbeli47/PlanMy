using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using AuditApp.Android;
using PlanMy.Droid;
using PlanMy.Models;
using Xamarin.Forms;

[assembly: Dependency(typeof(RateMyApp))]
namespace PlanMy.Droid
{
    class RateMyApp : IRateApp
    {
        
        public void RateThisApp()
        {
            var activity = Forms.Context as MainActivity;
            AndroidPlaystoreAudit.Instance.Run(activity);
        }
    }
}