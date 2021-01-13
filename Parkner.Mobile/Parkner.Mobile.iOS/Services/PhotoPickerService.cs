using Parkner.Mobile.iOS.Services;
using Parkner.Mobile.Services;
using System;
using System.IO;
using System.Threading.Tasks;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(PhotoPickerService))]

namespace Parkner.Mobile.iOS.Services
{
    public class PhotoPickerService : IPhotoPickerService
    {
        private UIImagePickerController FotoPicka { get; set; }
        private TaskCompletionSource<Stream> CompletionSource { get; set; }

        public async Task<Stream> ObtnerImagenAsync()
        {
            this.FotoPicka = new UIImagePickerController
            {
                SourceType = UIImagePickerControllerSourceType.PhotoLibrary,
                MediaTypes = UIImagePickerController.AvailableMediaTypes(UIImagePickerControllerSourceType.PhotoLibrary)
            };

            this.FotoPicka.FinishedPickingMedia += this.Finalizado;
            this.FotoPicka.Canceled += this.Cancelado;

            UIWindow ventana = UIApplication.SharedApplication.KeyWindow;

            if (ventana.RootViewController != null)
            {
                UIViewController viewControlla = ventana.RootViewController;
                viewControlla.PresentViewController(this.FotoPicka, true, null);
            }

            this.CompletionSource = new TaskCompletionSource<Stream>();

            return await this.CompletionSource.Task;
        }

        private void Finalizado(object sender, UIImagePickerMediaPickedEventArgs args)
        {
            UIImage imagen = args.EditedImage ?? args.OriginalImage;

            if (imagen != null)
            {
                this.DesubscribirEventos();

                this.CompletionSource.SetResult((args.ReferenceUrl.PathExtension.Equals("PNG") || args.ReferenceUrl.PathExtension.Equals("png") ? imagen.AsPNG() : imagen.AsJPEG(1)).AsStream());
            }
            else
            {
                this.DesubscribirEventos();
                this.CompletionSource.SetResult(null);
            }
            this.FotoPicka.DismissModalViewController(true);
        }

        private void Cancelado(object sender, EventArgs args)
        {
            this.DesubscribirEventos();
            this.CompletionSource.SetResult(null);
            this.FotoPicka.DismissModalViewController(true);
        }

        private void DesubscribirEventos()
        {
            this.FotoPicka.FinishedPickingMedia -= this.Finalizado;
            this.FotoPicka.Canceled -= this.Cancelado;
        }
    }
}
