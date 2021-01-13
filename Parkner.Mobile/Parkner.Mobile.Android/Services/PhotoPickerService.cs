using Android.Content;
using Parkner.Mobile.Droid.Services;
using Parkner.Mobile.Services;
using System.IO;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]

namespace Parkner.Mobile.Droid.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public Task<Stream> ObtnerImagenAsync()
        {
            Intent intent = new Intent();
            intent.SetType("image/*");
            intent.SetAction(Intent.ActionGetContent);

            MainActivity.Instancia.StartActivityForResult
            (
                Intent.CreateChooser(intent, "Seleccione una foto"),
                MainActivity.PickImageId
            );

            MainActivity.Instancia.ElegirImagenTaskCompletionSource = new TaskCompletionSource<Stream>();

            return MainActivity.Instancia.ElegirImagenTaskCompletionSource.Task;
        }
    }
}
