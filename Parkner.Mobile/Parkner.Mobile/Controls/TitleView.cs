using System;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Controls
{
    /// <summary>
    ///     The Title view
    /// </summary>
    [Preserve(AllMembers = true)]
    public class TitleView : Grid
    {
        #region variables
        /// <summary>
        ///     Gets or sets the title label.
        /// </summary>
        private Label titleLabel;
        #endregion

        #region Constructor
        public TitleView()
        {
            this.ColumnSpacing = 2;
            this.RowSpacing = 8;
            this.Padding = new Thickness(0, 8, 0, 0);

            this.ColumnDefinitions = new ColumnDefinitionCollection
            {
                new() {Width = 8},
                new(),
                new(),
                new(),
                new() {Width = 8}
            };

            this.RowDefinitions = new RowDefinitionCollection
            {
                new() {Height = GridLength.Auto},
                new() {Height = 1}
            };

            BoxView boxView = new() {Color = Color.FromHex("#ebecef")};

            this.Children.Add(this.LeadingView, 1, 0);
            this.Children.Add(this.Content, 2, 0);
            this.Children.Add(this.TrailingView, 3, 0);
            this.Children.Add(boxView, 0, 1);
            Grid.SetColumnSpan(boxView, 5);
        }
        #endregion

        #region Bindable Properties
        /// <summary>
        ///     Gets or sets the LeadingViewProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty LeadingViewProperty = BindableProperty.Create(nameof(TitleView.LeadingView), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, TitleView.OnLeadingViewPropertyChanged);

        /// <summary>
        ///     Gets or sets the TrailingViewProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty TrailingViewProperty = BindableProperty.Create(nameof(TitleView.TrailingView), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, TitleView.OnTrailingViewPropertyChanged);

        /// <summary>
        ///     Gets or sets the ContentProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty ContentProperty = BindableProperty.Create(nameof(TitleView.Content), typeof(View), typeof(TitleView), new ContentView(), BindingMode.Default, null, TitleView.OnContentPropertyChanged);

        /// <summary>
        ///     Gets or sets the TitleProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty TitleProperty = BindableProperty.Create(nameof(TitleView.Title), typeof(string), typeof(TitleView), String.Empty, BindingMode.Default, null, TitleView.OnTitlePropertyChanged);

        /// <summary>
        ///     Gets or sets the FontFamilyProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty FontFamilyProperty = BindableProperty.Create(nameof(TitleView.FontFamily), typeof(string), typeof(TitleView), String.Empty, BindingMode.Default, null, TitleView.OnFontFamilyPropertyChanged);

        /// <summary>
        ///     Gets or sets the FontAttributesProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty FontAttributesProperty = BindableProperty.Create(nameof(TitleView.FontAttributes), typeof(FontAttributes), typeof(TitleView), FontAttributes.None, BindingMode.Default, null, TitleView.OnFontAttributesPropertyChanged);

        /// <summary>
        ///     Gets or sets the FontSizeProperty, and it is a bindable property.
        /// </summary>
        public static readonly BindableProperty FontSizeProperty = BindableProperty.Create(nameof(TitleView.FontSize), typeof(double), typeof(TitleView), 16d, BindingMode.Default, null, TitleView.OnFontSizePropertyChanged);
        #endregion

        #region Public Properties
        /// <summary>
        ///     Gets or sets the LeadingView.
        /// </summary>
        public View LeadingView
        {
            get => (View)this.GetValue(TitleView.LeadingViewProperty);
            set => this.SetValue(TitleView.LeadingViewProperty, value);
        }

        /// <summary>
        ///     Gets or sets the TrailingView.
        /// </summary>
        public View TrailingView
        {
            get => (View)this.GetValue(TitleView.TrailingViewProperty);
            set => this.SetValue(TitleView.TrailingViewProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Content.
        /// </summary>
        public View Content
        {
            get => (View)this.GetValue(TitleView.ContentProperty);
            set => this.SetValue(TitleView.ContentProperty, value);
        }

        /// <summary>
        ///     Gets or sets the Title.
        /// </summary>
        public string Title
        {
            get => (string)this.GetValue(TitleView.TitleProperty);
            set => this.SetValue(TitleView.TitleProperty, value);
        }

        /// <summary>
        ///     Gets or sets the FontFamily.
        /// </summary>
        public string FontFamily
        {
            get => (string)this.GetValue(TitleView.FontFamilyProperty);
            set => this.SetValue(TitleView.FontFamilyProperty, value);
        }

        /// <summary>
        ///     Gets or sets the FontAttributes.
        /// </summary>
        public FontAttributes FontAttributes
        {
            get => (FontAttributes)this.GetValue(TitleView.FontAttributesProperty);
            set => this.SetValue(TitleView.FontAttributesProperty, value);
        }

        /// <summary>
        ///     Gets or sets the FontSize.
        /// </summary>
        public double FontSize
        {
            get => (double)this.GetValue(TitleView.FontSizeProperty);
            set => this.SetValue(TitleView.FontSizeProperty, value);
        }
        #endregion

        #region Methods
        /// <summary>
        ///     Invoked when the leading view is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnLeadingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;
            View newView = (View)newValue;
            newView.HorizontalOptions = LayoutOptions.Start;
            titleView.Children.Add(newView, 1, 0);
        }

        /// <summary>
        ///     Invoked when the trailing view is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnTrailingViewPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;
            View newView = (View)newValue;
            newView.HorizontalOptions = LayoutOptions.End;
            titleView.Children.Add(newView, 3, 0);
        }

        /// <summary>
        ///     Invoked when the Content is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnContentPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;
            View newView = (View)newValue;

            if (!String.IsNullOrEmpty(titleView.Title)) titleView.Children.Remove(titleView.titleLabel);

            titleView.Children.Add(newView, 2, 0);
        }

        /// <summary>
        ///     Invoked when the Title is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnTitlePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;
            string newText = (string)newValue;

            if (!String.IsNullOrEmpty(newText))
            {
                titleView.titleLabel = new Label
                {
                    Text = newText,
                    TextColor = Color.FromHex("#333942"),//(Color)Application.Current.Resources["Gray-900"],
                    FontSize = 16,
                    Margin = new Thickness(0, 8),
                    FontFamily = Device.RuntimePlatform == Device.Android
                        ? "Montserrat-Medium.ttf#Montserrat-Medium"
                        : Device.RuntimePlatform == Device.iOS
                            ? "Montserrat-Medium"
                            : "Assets/Montserrat-Medium.ttf#Montserrat-Medium",
                    HorizontalTextAlignment = TextAlignment.Center,
                    VerticalTextAlignment = TextAlignment.Center,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center
                };

                if (Device.RuntimePlatform == Device.Android) titleView.titleLabel.LineHeight = 1.5;

                titleView.Children.Remove(titleView.Content);
                titleView.Children.Add(titleView.titleLabel, 2, 0);
            }
        }

        /// <summary>
        ///     Invoked when the FontFamily is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnFontFamilyPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;

            if (titleView.titleLabel != null) titleView.titleLabel.FontFamily = (string)newValue;
        }

        /// <summary>
        ///     Invoked when the FontAttributes is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnFontAttributesPropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;

            if (titleView.titleLabel != null) titleView.titleLabel.FontAttributes = (FontAttributes)newValue;
        }

        /// <summary>
        ///     Invoked when the FontSize is changed.
        /// </summary>
        /// <param name="bindable">The TitleView</param>
        /// <param name="oldValue">The old value</param>
        /// <param name="newValue">The new value</param>
        private static void OnFontSizePropertyChanged(BindableObject bindable, object oldValue, object newValue)
        {
            TitleView titleView = bindable as TitleView;

            if (titleView.titleLabel != null) titleView.titleLabel.FontSize = (double)newValue;
        }
        #endregion
    }
}
