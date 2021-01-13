using Android.Content;
using Android.Runtime;
using Android.Views;
using Parkner.Mobile.Droid.Services;
using Parkner.Mobile.Services;
using Xamarin.Forms;
using Application = Android.App.Application;

[assembly: Dependency(typeof(DeviceOrientationService))]

namespace Parkner.Mobile.Droid.Services
{
    public class DeviceOrientationService : IDeviceOrientationService
    {
        public DeviceOrientation GetOrientation()
        {
            IWindowManager windowManager = Application.Context.GetSystemService(Context.WindowService).JavaCast<IWindowManager>();
            
            if (windowManager?.DefaultDisplay is null) return DeviceOrientation.Portrait;

            SurfaceOrientation orientation = windowManager.DefaultDisplay.Rotation;
            bool isLandscape = (orientation == SurfaceOrientation.Rotation90) ||
                               (orientation == SurfaceOrientation.Rotation270);
            return isLandscape ? DeviceOrientation.Landscape : DeviceOrientation.Portrait;
        }
    }
}
