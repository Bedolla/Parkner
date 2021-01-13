using System;
using System.Globalization;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Converters
{
    [Preserve(AllMembers = true)]
    public class DateTimeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            DateTime currentTime = DateTime.Now;
            DateTime dateTime = (DateTime)value;

            return dateTime.Day == currentTime.Day ? "Hoy" : dateTime.Day == currentTime.AddDays(-1).Day ? "Ayer" : dateTime.ToString("MMMM dd, yyyy", CultureInfo.CurrentCulture);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;
    }
}
