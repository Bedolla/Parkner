using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Converters
{
    [Preserve(AllMembers = true)]
    public class StringToGlyphConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            object text = value;
            switch ((string)text)
            {
                case "Text":
                    ((Label)parameter).IsVisible = false;
                    return text;
                case "Viewed":
                case "New":
                    Application.Current.Resources.TryGetValue("PrimaryColor", out object retVal);
                    if (retVal != null) ((Label)parameter).TextColor = (Color)retVal;
                    break;
                case "Received":
                case "Sent":
                    Application.Current.Resources.TryGetValue("Gray-600", out object colorVal);
                    if (colorVal != null) ((Label)parameter).TextColor = (Color)colorVal;
                    break;
                case "Audio":
                case "Video":
                case "Contact":
                case "Photo":
                    ((Label)parameter).IsVisible = true;
                    break;
            }

            ((Label)parameter).Resources.TryGetValue((string)value, out text);
            return text;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
