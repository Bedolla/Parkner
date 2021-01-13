using Android.Views;
using Parkner.Mobile.Droid.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Application = Android.App.Application;

[assembly: ExportRenderer(typeof(Parkner.Mobile.Controls.BorderlessEntry), typeof(BorderlessEntryRenderer))]

namespace Parkner.Mobile.Droid.Renderers
{
    public class BorderlessEntryRenderer : EntryRenderer
    {
        public BorderlessEntryRenderer() : base(Application.Context)
        {
        }

        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (this.Control == null) return;

            this.Control.SetBackground(null);
            this.Control.Gravity = GravityFlags.CenterVertical;
            this.Control.SetPadding(0, 0, 0, 0);
        }
    }
}