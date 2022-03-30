////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CustomEntryRenderer.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Custom Entry class for Android Platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.iOS.Implementations;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomEntry), typeof(CustomEntryRenderer))]
namespace engie_maintenance_app.iOS.Implementations
{
    public class CustomEntryRenderer : EntryRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Entry> e)
        {
            base.OnElementChanged(e);

            if (Control != null)
            {
                Control.BorderStyle = UITextBorderStyle.Bezel;
                Control.Layer.CornerRadius = 15;
                Control.Layer.BorderColor = Settings.EngieBlueColor.ToCGColor();
                Control.Layer.BorderWidth = 2;
            }
        }
    }
}