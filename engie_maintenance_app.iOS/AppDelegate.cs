////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: AppDelegate.cs
//FileType: Visual C# Source file
//Author : Your name here
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Write here what the class is for
////////////////////////////////////////////////////////////////////////////////////////////////////////
using engie_maintenance_app.Views;
using FFImageLoading.Forms.Platform;
using Foundation;
using Syncfusion.ListView.XForms.iOS;
using Syncfusion.SfPdfViewer.XForms.iOS;
using Syncfusion.SfRangeSlider.XForms.iOS;
using Syncfusion.XForms.iOS.EffectsView;
using Syncfusion.XForms.iOS.Expander;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;
using Platform = ZXing.Net.Mobile.Forms.iOS.Platform;

namespace engie_maintenance_app.iOS
{
    /// <summary>
    /// The UIApplicationDelegate for the application. This class is responsible for launching the 
    /// User Interface of the application, as well as listening (and optionally responding) to 
    /// application events from iOS.
    /// </summary>
    [Register("AppDelegate")]
    public class AppDelegate : FormsApplicationDelegate
    {
        /// <summary>
        /// This method is invoked when the application has loaded and is ready to run. In this 
        /// method you should instantiate the window, load the UI into it and then make the window
        /// visible.
        ///  
        /// You have 17 seconds to return from this method, or iOS will terminate your application.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            Forms.Init();
            Platform.Init();
            CachedImageRenderer.Init();
            SfPdfDocumentViewRenderer.Init();
            SfRangeSliderRenderer.Init();
            SfExpanderRenderer.Init(); 
            SfListViewRenderer.Init();
            SfEffectsViewRenderer.Init();  //Initialize only when effects view is added to Listview.
            LoadApplication(new App());

            return base.FinishedLaunching(app, options);
        }
    }
}