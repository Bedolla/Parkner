using Android;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Syncfusion.XForms.Android.PopupLayout;
using System.IO;
using System.Threading.Tasks;
using Xamarin;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Platform = Xamarin.Essentials.Platform;

namespace Parkner.Mobile.Droid
{
    [Activity(Label = "Parkner", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        public const int PickImageId = 1000;

        internal static MainActivity Instancia { get; private set; }

        public TaskCompletionSource<Stream> ElegirImagenTaskCompletionSource { set; get; }

        private const int RequestLocationId = 0;

        private readonly string[] LocationPermissions =
        {
            Manifest.Permission.AccessCoarseLocation,
            Manifest.Permission.AccessFineLocation
        };

        protected override void OnStart()
        {
            base.OnStart();

            if ((int)Build.VERSION.SdkInt >= 23)
                if (this.CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
                    this.RequestPermissions(this.LocationPermissions, MainActivity.RequestLocationId);
        }

        protected override void OnCreate(Bundle savedInstanceState)
        {
            FormsAppCompatActivity.TabLayoutResource = Resource.Layout.Tabbar;
            FormsAppCompatActivity.ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            MainActivity.Instancia = this;

            Forms.SetFlags("CollectionView_Experimental");

            Platform.Init(this, savedInstanceState);

            Forms.Init(this, savedInstanceState);

            FormsMaps.Init(this, savedInstanceState);

            SfPopupLayoutRenderer.Init();

            this.LoadApplication(new App());
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        protected override void OnActivityResult(int requestCode, Result resultCode, Intent intent)
        {
            base.OnActivityResult(requestCode, resultCode, intent);

            if (requestCode != MainActivity.PickImageId) return;

            this.ElegirImagenTaskCompletionSource.SetResult((resultCode == Result.Ok) && (intent != null) ? this.ContentResolver?.OpenInputStream(intent.Data) : null);
        }
    }
}
