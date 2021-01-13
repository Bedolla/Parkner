using System;
using System.Globalization;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Parkner.Mobile.Converters
{
    public class Base64ToImageConverter : IValueConverter
    {
        //<ContentPage.Resources>
        //    <ResourceDictionary>
        //        <local:Base64ToImageConverter x:Key="Base64ToImageConverter" />
        //    </ResourceDictionary>
        //</ContentPage.Resources>

        //<Image Source = "{Binding Foto, Converter={StaticResource Base64ToImageConverter}}}"
        //    HorizontalOptions="Center"
        //    HeightRequest="50" 
        //    VerticalOptions="Center">
        //
        //    <Image.GestureRecognizers>
        //        <TapGestureRecognizer Tapped="OnImageTapped" />
        //    </Image.GestureRecognizers>
        //
        //</Image>

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string imagenBase64String = (string)value;

            if (imagenBase64String is null) return null;

            byte[] imagenBytes = System.Convert.FromBase64String(imagenBase64String);

            return ImageSource.FromStream(() => new MemoryStream(imagenBytes));
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            StreamImageSource imageSource = (StreamImageSource)value;
            Task<Stream> streamTarea = imageSource.Stream(CancellationToken.None);
            Stream stream = streamTarea.Result;

            using MemoryStream ms = new MemoryStream();

            stream.CopyTo(ms);

            return System.Convert.ToBase64String(ms.ToArray());
        }
    }
}
