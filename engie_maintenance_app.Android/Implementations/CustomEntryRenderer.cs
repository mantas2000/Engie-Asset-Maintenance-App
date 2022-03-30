////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CustomEntryRenderer.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Custom Entry class for Android Platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using Android.Graphics.Drawables;
using engie_maintenance_app.Droid.Implementations;
using engie_maintenance_app.Interfaces;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer (typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace engie_maintenance_app.Droid.Implementations
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged (ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null) {
                var gd = new GradientDrawable();
                gd.SetCornerRadius(30);
                gd.SetStroke(5, Settings.EngieBlueColor.ToAndroid());
                Control.SetPadding(30,0,0,0);
                Control.SetBackgroundDrawable(gd);
            }
        }
    }
}