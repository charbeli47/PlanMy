using Xamarin.Forms;

using Xamarin.Forms.Platform.Android;
using PlanMy.Models;
using Android.Graphics.Drawables;
using PlanMy.Droid;

[assembly: ExportRenderer(typeof(searchevententry), typeof(searchevententryrenderer))]
namespace PlanMy.Droid
{
	class searchevententryrenderer : SearchBarRenderer
	{
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.SearchEventEntry);
            }
        }

	}
}