using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Foundation;
using PlanMy.iOS;
using PlanMy.Models;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(BaseUrl_iOS))]
namespace PlanMy.iOS
{
    public class BaseUrl_iOS : IBaseUrl
    {
        public string Get()
        {
            return NSBundle.MainBundle.BundlePath;
        }
    }
}