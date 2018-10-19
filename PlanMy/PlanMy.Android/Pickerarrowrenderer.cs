using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PlanMy.Models;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.Content;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using PlanMy.Droid;
using Android.Graphics.Drawables.Shapes;

[assembly: ExportRenderer(typeof(Pickerarrow), typeof(Pickerarrowrenderer))]
namespace PlanMy.Droid
{
	
	class Pickerarrowrenderer: PickerRenderer
	{
		Pickerarrow element;

		protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
		{
			base.OnElementChanged(e);

			element = (Pickerarrow)this.Element;

			if (Control != null && this.Element != null && !string.IsNullOrEmpty(element.Image))
				Control.Background = AddPickerStyles(element.Image);

		}

		public LayerDrawable AddPickerStyles(string imagePath)
		{

			GradientDrawable gd = new GradientDrawable();
			gd.SetColor(Android.Graphics.Color.White);
			gd.SetCornerRadius(25);
		
			gd.SetStroke(2, Android.Graphics.Color.Black);

			//ShapeDrawable border = new ShapeDrawable(new RectShape());
			//border.Paint.Color = Android.Graphics.Color.Black;
			//border.SetPadding(10, 10, 10, 10);
			//border.=
			//border.Paint.SetStyle(Paint.Style.Stroke);

			Drawable[] layers = { gd, GetDrawable(imagePath) };
			LayerDrawable layerDrawable = new LayerDrawable(layers);
			layerDrawable.SetLayerInset(0, 0, 0, 0, 0);

			return layerDrawable;
		}

		private Android.Graphics.Drawables.BitmapDrawable GetDrawable(string imagePath)
		{
			int resID = Resources.GetIdentifier(imagePath, "drawable", this.Context.PackageName);
			var drawable = ContextCompat.GetDrawable(this.Context, resID);
			
			var bitmap = ((BitmapDrawable)drawable).Bitmap;

			var result = new BitmapDrawable(Resources, Bitmap.CreateScaledBitmap(bitmap, 100, 50, true));
			result.Gravity = Android.Views.GravityFlags.Right;

			return result;
		}

	}
}