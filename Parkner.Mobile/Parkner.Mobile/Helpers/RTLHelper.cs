using Syncfusion.XForms.Border;
using Syncfusion.XForms.Cards;
using System.ComponentModel;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Helpers
{
    /// <summary>
    /// This class is used to set margin or padding based on flow direction (LTR or RTL).
    /// </summary>
    [Preserve(AllMembers = true)]
    public static class RTLHelper
    {
        #region Bindable Properties

        /// <summary>
        /// Gets or sets the MarginProperty.
        /// </summary>
        public static readonly BindableProperty MarginProperty = BindableProperty.CreateAttached("Margin",
            typeof(Thickness), typeof(RTLHelper), RTLHelper.ZeroThickness, propertyChanged: RTLHelper.OnMarginPropertyChanged);

        /// <summary>
        /// Gets or sets the PaddingProperty.
        /// </summary>
        public static readonly BindableProperty PaddingProperty = BindableProperty.CreateAttached("Padding",
            typeof(Thickness), typeof(RTLHelper), RTLHelper.ZeroThickness, propertyChanged: RTLHelper.OnPaddingPropertyChanged);

        /// <summary>
        /// Gets or sets the CornerRadiusProperty.
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.CreateAttached("CornerRadius",
            typeof(Thickness), typeof(RTLHelper), RTLHelper.ZeroThickness, propertyChanged: RTLHelper.OnCornerRadiusPropertyChanged);

        #endregion

        #region Private Fields

        /// <summary>
        /// Field to hold the zero thickness.
        /// </summary>
        private static readonly Thickness ZeroThickness = new Thickness();

        #endregion

        #region Methods

        /// <summary>
        /// Gets the value of margin.
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <returns>Returns the margin</returns>
        public static Thickness GetMargin(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(RTLHelper.MarginProperty);
        }

        /// <summary>
        /// Gets the value of padding.
        /// </summary>
        /// <param name="bindable">The layout</param>
        /// <returns>Returns the padding.</returns>
        public static Thickness GetPadding(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(RTLHelper.PaddingProperty);
        }

        /// <summary>
        /// Gets the value of corner radius. 
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <returns>Returns the corner radius.</returns>
        public static Thickness GetCornerRadius(BindableObject bindable)
        {
            return (Thickness)bindable?.GetValue(RTLHelper.CornerRadiusProperty);
        }

        /// <summary>
        /// Sets the value of margin.
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <param name="value">The margin</param>
        public static void SetMargin(BindableObject bindable, Thickness value)
        {
            if (value != RTLHelper.ZeroThickness)
            {
                bindable?.SetValue(RTLHelper.MarginProperty, value);
            }
            else
            {
                bindable?.ClearValue(RTLHelper.MarginProperty);
            }
        }

        /// <summary>
        /// Sets the value of padding.
        /// </summary>
        /// <param name="bindable">The layout</param>
        /// <param name="value">The padding</param>
        public static void SetPadding(BindableObject bindable, Thickness value)
        {
            if (value != RTLHelper.ZeroThickness)
            {
                bindable?.SetValue(RTLHelper.PaddingProperty, value);
            }
            else
            {
                bindable?.ClearValue(RTLHelper.PaddingProperty);
            }
        }

        /// <summary>
        /// Sets the value of corner radius.
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <param name="value">The corner radius</param>
        public static void SetCornerRadius(BindableObject bindable, Thickness value)
        {
            if (value != RTLHelper.ZeroThickness)
            {
                bindable?.SetValue(RTLHelper.CornerRadiusProperty, value);
            }
            else
            {
                bindable?.ClearValue(RTLHelper.CornerRadiusProperty);
            }
        }

        /// <summary>
        /// Invoked when the value of margin property is changed.
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">the new value</param>
        private static void OnMarginPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view)) return;

            var previousMargin = (Thickness)oldValue;
            var currentMargin = (Thickness)newValue;

            RTLHelper.UpdateMargin(view);

            if (currentMargin != RTLHelper.ZeroThickness)
            {
                if (previousMargin == RTLHelper.ZeroThickness)
                {
                    RTLHelper.OnElementAttached(view);
                }
            }
            else
            {
                RTLHelper.OnElementDetached(view);
            }
        }

        /// <summary>
        /// Invoked when the value of padding property is changed.
        /// </summary>
        /// <param name="bindable">The layout</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">the new value</param>
        private static void OnPaddingPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is Layout layout)) return;

            var previousPadding = (Thickness)oldValue;
            var currentPadding = (Thickness)newValue;

            RTLHelper.UpdatePadding(layout);

            if (currentPadding != RTLHelper.ZeroThickness)
            {
                if (previousPadding == RTLHelper.ZeroThickness)
                {
                    RTLHelper.OnElementAttached(layout);
                }
            }
            else
            {
                RTLHelper.OnElementDetached(layout);
            }
        }

        /// <summary>
        /// Invoked when the value of corner radius property is changed.
        /// </summary>
        /// <param name="bindable">The view</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">the new value</param>
        private static void OnCornerRadiusPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            if (!(bindable is View view)) return;

            var previousCornerRadius = (Thickness)oldValue;
            var currentCornerRadius = (Thickness)newValue;

            RTLHelper.UpdateCornerRadius(view);

            if (currentCornerRadius != RTLHelper.ZeroThickness)
            {
                if (previousCornerRadius == RTLHelper.ZeroThickness)
                {
                    RTLHelper.OnElementAttached(view);
                }
            }
            else
            {
                RTLHelper.OnElementDetached(view);
            }
        }

        /// <summary>
        /// Updates the margin value when the flow direction is changed.
        /// </summary>
        /// <param name="view">The view</param>
        private static void UpdateMargin(VisualElement view)
        {
            var controller = (IVisualElementController)view;
            var margin = RTLHelper.GetMargin(view);

            if (margin != RTLHelper.ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    margin = new Thickness(margin.Right, margin.Top, margin.Left, margin.Bottom);
                }

                view.SetValue(View.MarginProperty, margin);
            }
            else
            {
                view.ClearValue(View.MarginProperty);
            }
        }

        /// <summary>
        /// Updates padding when the the flow direction is changed.
        /// </summary>
        /// <param name="layout">The layout</param>
        private static void UpdatePadding(View layout)
        {
            var controller = (IVisualElementController)layout;
            var padding = RTLHelper.GetPadding(layout);

            if (padding != RTLHelper.ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    padding = new Thickness(padding.Right, padding.Top, padding.Left, padding.Bottom);
                }

                layout.SetValue(Layout.PaddingProperty, padding);
            }
            else
            {
                layout.ClearValue(Layout.PaddingProperty);
            }
        }


        /// <summary>
        /// Updates the value of the corner radius when flow direction is changed.
        /// </summary>
        /// <param name="view">The view</param>
        private static void UpdateCornerRadius(View view)
        {
            var controller = (IVisualElementController)view;
            var cornerRadius = RTLHelper.GetCornerRadius(view);

            if (cornerRadius != RTLHelper.ZeroThickness)
            {
                if (controller.EffectiveFlowDirection == EffectiveFlowDirection.RightToLeft)
                {
                    cornerRadius = new Thickness(cornerRadius.Top, cornerRadius.Left, cornerRadius.Bottom,
                        cornerRadius.Right);
                }

                if (view is SfCardView)
                {
                    view.SetValue(SfCardView.CornerRadiusProperty, cornerRadius);
                }
                else if (view is SfBorder)
                {
                    view.SetValue(SfBorder.CornerRadiusProperty, cornerRadius);
                }
            }
            else
            {
                if (view is SfCardView)
                {
                    view.ClearValue(SfCardView.CornerRadiusProperty);
                }
                else if (view is SfBorder)
                {
                    view.ClearValue(SfBorder.CornerRadiusProperty);
                }
            }
        }


        /// <summary>
        /// Updates the margin when flow direction is changed .
        /// </summary>
        /// <param name="sender">The view</param>
        /// <param name="e">Property changed event args</param>
        private static void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (sender is View view)
            {
                if (e.PropertyName == VisualElement.FlowDirectionProperty.PropertyName)
                {
                    RTLHelper.UpdateMargin(view);
                    RTLHelper.UpdatePadding(view);
                    RTLHelper.UpdateCornerRadius(view);
                }
            }
        }

        /// <summary>
        /// Invoked when the view is added to the main view.
        /// </summary>
        /// <param name="element">The view</param>
        private static void OnElementAttached(View element)
        {
            element.PropertyChanged += RTLHelper.OnElementPropertyChanged;
        }

        /// <summary>
        /// Invoked when detach from the view.
        /// </summary>
        /// <param name="element">The view</param>
        private static void OnElementDetached(View element)
        {
            element.PropertyChanged -= RTLHelper.OnElementPropertyChanged;
        }

        #endregion
    }
}