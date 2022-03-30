////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: CustomPickerRenderer.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Custom Picker class for Android Platform
////////////////////////////////////////////////////////////////////////////////////////////////////////
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.iOS.Implementations;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(CustomPicker), typeof(CustomPickerRenderer))]
namespace engie_maintenance_app.iOS.Implementations
{
    public class CustomPickerRenderer : PickerRenderer
    {
        protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
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