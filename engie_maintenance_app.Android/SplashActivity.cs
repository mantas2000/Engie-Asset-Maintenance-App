////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CustomPickerRenderer.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class for Splash Screen on Android Platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Android.App;
using Android.Content;
using Android.OS;
using Android.Support.V7.App;

namespace engie_maintenance_app.Droid
{
    [Activity(Theme = "@style/MyTheme.Splash", MainLauncher = true, NoHistory = true)]
    public class SplashActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            StartActivity(new Intent(Application.Context, typeof (MainActivity)));
            Finish();
        }

        // Prevent the back button from canceling the startup process
        public override void OnBackPressed() {}
    }
}