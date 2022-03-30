using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using engie_maintenance_app.Views;
using FFImageLoading.Forms.Platform;
using Plugin.Fingerprint;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using ZXing.Net.Mobile.Android;
using Platform = Xamarin.Essentials.Platform;

namespace engie_maintenance_app.Droid
{
    [Activity(Label = "Engie Maintenance", Icon = "@mipmap/icon", Theme = "@style/MyTheme.Splash", LaunchMode = LaunchMode.SingleTop, MainLauncher = false, ScreenOrientation = ScreenOrientation.Portrait, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
    public class MainActivity : FormsAppCompatActivity
    {
        private readonly string[] _permissionGroup = 
        {
            Manifest.Permission.Internet,
            Manifest.Permission.ReadExternalStorage,
            Manifest.Permission.WriteExternalStorage,
            Manifest.Permission.Flashlight,
            Manifest.Permission.Camera,
        };
        protected override void OnCreate(Bundle savedInstanceState)
        {
            CrossFingerprint.SetCurrentActivityResolver(() => this);
            base.SetTheme(Resource.Style.MainTheme);
            base.OnCreate(savedInstanceState);
            
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            Platform.Init(this, savedInstanceState);
            Forms.Init(this, savedInstanceState);
            ZXing.Net.Mobile.Forms.Android.Platform.Init();
            CachedImageRenderer.Init(true);   
            LoadApplication(new App());
            
            RequestPermissions(_permissionGroup,0);
        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Permission[] grantResults)
        {
            Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            PermissionsHandler.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}
