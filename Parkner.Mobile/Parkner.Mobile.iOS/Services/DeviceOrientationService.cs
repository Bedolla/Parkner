using Parkner.Mobile.iOS.Services;
using Parkner.Mobile.Services;
using UIKit;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationService))]

namespace Parkner.Mobile.iOS.Services
{
    public class DeviceOrientationService : IDeviceOrientationService
    {
        public DeviceOrientation GetOrientation()
        {
            UIInterfaceOrientation orientation = UIApplication.SharedApplication.StatusBarOrientation;

            bool isPortrait = (orientation == UIInterfaceOrientation.Portrait) ||
                              (orientation == UIInterfaceOrientation.PortraitUpsideDown);
            return isPortrait ? DeviceOrientation.Portrait : DeviceOrientation.Landscape;
        }
    }
}
