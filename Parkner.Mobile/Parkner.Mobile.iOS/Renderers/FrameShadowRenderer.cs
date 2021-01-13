using CoreGraphics;
using Parkner.Mobile.Controls;
using Parkner.Mobile.iOS.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomShadowFrame), typeof(FrameShadowRenderer))]

namespace Parkner.Mobile.iOS.Renderers
{
    public class FrameShadowRenderer : FrameRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Frame> element)
        {
            base.OnElementChanged(element);
            CustomShadowFrame customShadowFrame = (CustomShadowFrame)this.Element;
            if (customShadowFrame == null) return;
            this.Layer.CornerRadius = customShadowFrame.Radius;
            this.Layer.ShadowOpacity = customShadowFrame.ShadowOpacity;
            this.Layer.ShadowOffset = new CGSize(customShadowFrame.ShadowOffsetWidth, customShadowFrame.ShadowOffSetHeight);
            this.Layer.Bounds.Inset(customShadowFrame.BorderWidth, customShadowFrame.BorderWidth);
            this.Layer.BorderColor = customShadowFrame.CustomBorderColor.ToCGColor();
            this.Layer.BorderWidth = (float)customShadowFrame.BorderWidth;
        }
    }
}