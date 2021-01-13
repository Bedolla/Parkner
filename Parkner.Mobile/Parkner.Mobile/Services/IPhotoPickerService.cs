using System.IO;
using System.Threading.Tasks;

namespace Parkner.Mobile.Services
{
    public interface IPhotoPickerService
    {
        Task<Stream> ObtnerImagenAsync();
    }
}
