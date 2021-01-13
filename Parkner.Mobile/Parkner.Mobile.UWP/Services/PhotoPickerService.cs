using Parkner.Mobile.Services;
using Parkner.Mobile.UWP.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Storage.Pickers;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]

namespace Parkner.Mobile.UWP.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        public async Task<Stream> ObtnerImagenAsync()
        {
            StorageFile almacenamientoDeArchivos = await new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".jpg",
                    ".jpeg",
                    ".png",
                    ".gif"
                }
            }.PickSingleFileAsync();

            return almacenamientoDeArchivos == null ? null : (await almacenamientoDeArchivos.OpenReadAsync()).AsStreamForRead();
        }
    }
}
