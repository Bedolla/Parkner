using Parkner.Mobile.Services;
using Parkner.Mobile.UWP.Services;
using Windows.UI.ViewManagement;
using Xamarin.Forms;

[assembly: Dependency(typeof(DeviceOrientationService))]

namespace Parkner.Mobile.UWP.Services
{
    public class DeviceOrientationService : IDeviceOrientationService
    {
        public DeviceOrientation GetOrientation() => ApplicationView.GetForCurrentView().Orientation == ApplicationViewOrientation.Landscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
    }
}
