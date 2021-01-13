using Parkner.Mobile.Controls;
using Parkner.Mobile.ViewModels;
using System;
using System.Globalization;
using System.Text.RegularExpressions;
using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace Parkner.Mobile.Converters
{
    [Preserve(AllMembers = true)]
    public class ErrorValidationColorConverter : IValueConverter
    {
        public string PageVariantParameter { get; set; }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (this.PageVariantParameter == "0")
            {
                BorderlessEntry emailEntry = parameter as BorderlessEntry;

                if (!(emailEntry?.BindingContext is LoginViewModel bindingContext)) return Color.Transparent;

                bool isFocused = (bool)value;
                bindingContext.IsInvalidEmail = !isFocused && !ErrorValidationColorConverter.CheckValidEmail(bindingContext.Email);

                if (isFocused) return Color.FromRgba(255, 255, 255, 0.6);

                return bindingContext.IsInvalidEmail ? Color.FromHex("#FF4A4A") : Color.Transparent;
            }
            else
            {
                BorderlessEntry emailEntry = parameter as BorderlessEntry;

                if (!(emailEntry?.BindingContext is LoginViewModel bindingContext)) return Color.FromHex("#ced2d9");

                bool isFocused1 = (bool)value;
                bindingContext.IsInvalidEmail = !isFocused1 && !ErrorValidationColorConverter.CheckValidEmail(bindingContext.Email);

                if (isFocused1) return Color.FromHex("#959eac");

                return bindingContext.IsInvalidEmail ? Color.FromHex("#FF4A4A") : Color.FromHex("#ced2d9");
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => null;

        private static bool CheckValidEmail(string email)
        {
            if (String.IsNullOrEmpty(email)) return true;

            Regex regex = new Regex(@"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*");
            return regex.IsMatch(email) && !email.EndsWith(".");
        }
    }
}
