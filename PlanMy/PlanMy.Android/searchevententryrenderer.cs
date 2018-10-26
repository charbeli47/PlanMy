using Xamarin.Forms;

using Xamarin.Forms.Platform.Android;
using PlanMy.Models;
using Android.Graphics.Drawables;
using PlanMy.Droid;

[assembly: ExportRenderer(typeof(searchevententry), typeof(searchevententryrenderer))]
namespace PlanMy.Droid
{
	class searchevententryrenderer : EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.SearchEventEntry);
			}
		}

	}
}