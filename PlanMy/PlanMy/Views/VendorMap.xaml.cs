using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL.Models;
using Xamarin.Forms;
using Xamarin.Forms.Maps;
using Xamarin.Forms.Xaml;

namespace PlanMy.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VendorMap : ContentPage
    {
        Map map;
        public VendorMap(Item vendor, double latitude, double longitude)
        {
            InitializeComponent();
            Pagetitle.Text = WebUtility.HtmlDecode(vendor.Title.Rendered) + "'s location";
            map = new Map
            {
                IsShowingUser = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand
            };
            // You can use MapSpan.FromCenterAndRadius   
            map.MoveToRegion(MapSpan.FromCenterAndRadius(new Position(latitude, longitude), Distance.FromMiles(0.3)));
            stack.Children.Add(map);
        }

        private void backarrow_Clicked(object sender, EventArgs e)
        {
            Navigation.PopModalAsync();
        }
    }
}