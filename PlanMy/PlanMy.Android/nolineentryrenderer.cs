using Xamarin.Forms;

using Xamarin.Forms.Platform.Android;
using PlanMy.Models;
using Android.Graphics.Drawables;
using PlanMy.Droid;

[assembly: ExportRenderer(typeof(nolineentry), typeof(nolineentryrenderer))]
namespace PlanMy.Droid
{
	class nolineentryrenderer:EntryRenderer
	{
		protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
		{
			base.OnElementChanged(e);
			if (Control != null)
			{
				this.Control.Background = this.Resources.GetDrawable(Resource.Drawable.RoundedCornerEntry);
			}
		}

	}
}