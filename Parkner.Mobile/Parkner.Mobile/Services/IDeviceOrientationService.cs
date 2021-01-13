namespace Parkner.Mobile.Services
{
    public interface IDeviceOrientationService
    {
        DeviceOrientation GetOrientation();
    }
    public enum DeviceOrientation
    {
        Undefined,
        Landscape,
        Portrait
    }
}
