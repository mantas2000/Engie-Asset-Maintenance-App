////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: QrScanningPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to scan qr codes and open form linked to that. 
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using engie_maintenance_app.Interfaces;
using Plugin.Media;
using Plugin.Media.Abstractions;
using Xamarin.Forms;
using ZXing;

namespace engie_maintenance_app.Views
{
    public partial class QrScanningPage
    {
        private string _resultString;
        public QrScanningPage()
        {
            InitializeComponent();
            BindingContext = new GlobalSettings();
        }

        /// <summary>
        /// Asks the user what they want to do with the given Result.
        /// </summary>
        /// <param name="result">The QR code from the scanner</param>
        private void ZxingOnScanResult(Result result)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                // Sets the scanner off and shows loading.
                ScanView.IsScanning = false;
                ActIndicator.IsRunning = true;
                ActIndicator.IsVisible = true;

                // Gets the qr scan result.
                _resultString = result.Text;

                // Asks the user to open the form or cancel.
                var userResponse = await DisplayAlert("Open :", _resultString, "Open", "Cancel");

                // If user selected open opens the form else removes the qr scan page from stack and goes to main page.
                if (userResponse)
                {
                    OpenPdf(_resultString);
                }
                else
                {
                    // If user cancels removes the qr code page from stack and begin from beginning .
                    await Navigation.PopAsync();
                }
            });
        }

        /// <summary>
        /// Opens the pdf of the given uri.
        /// </summary>
        /// <param name="result">The uri of the form to open</param>
        async void OpenPdf(string result)
        {
            // Downloads the file from aws bucket if not exists, else just returns the filepath.
            string filePath = AwsBucket.AwsBucket.DownloadPdf(result);

            if (!filePath.Equals("failed"))
            {
                // Removes the qr code scanner page from the stack so when user backs out they go to main page.
                await Navigation.PushAsync(new PdfViewerPage(filePath));
                Navigation.RemovePage(this.Navigation.NavigationStack[this.Navigation.NavigationStack.Count - 2]);
            }
            else
            {
                // If fails removes goes to main page.
                await DisplayAlert("Error", "Failed to download file/open local file. Do you have an internet connection?", "Ok");
                await Navigation.PopAsync();
            }
        }

        /// <summary>
        /// Toggles the flashlight.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        async void FlashButtonClicked(object sender, EventArgs e)
        {
            try
            {
                ScanView.IsTorchOn = !ScanView.IsTorchOn;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                await DisplayAlert("Error", "Could not turn flashlight on/off", "OK");
            }
        }

        /// <summary>
        /// Opens file from local device's gallary
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private void SelectFromGalleryClicked(object sender, EventArgs e)
        {
            // Calls the function to select image and decodes the qr code
            SelectAndDecodeImage();
        }

        /// <summary>
        /// Selects image from gallery and reads the QR code from it.
        /// </summary>
        async void SelectAndDecodeImage()
        {
            // Selects image from gallery and read the qr code from it.
            try
            {
                await CrossMedia.Current.Initialize();

                if (!CrossMedia.Current.IsPickPhotoSupported)
                {
                    Console.WriteLine("This device doesn't support ...");
                    return;
                }
                
                // Open gallery/photos and load the select image into file variable.
                var file = await CrossMedia.Current.PickPhotoAsync(new PickMediaOptions
                {
                    PhotoSize = PhotoSize.Small,
                    CompressionQuality = 40
                });

                // If user selected a file.
                if (file != null)
                {

                    // Stops scanning and shows loading.
                    ScanView.IsScanning = false;
                    ActIndicator.IsRunning = true;
                    ActIndicator.IsVisible = true;

                    // Gets byte array of the selected image.
                    byte[] imageArray = File.ReadAllBytes(file.Path);

                    // Creates a qr code reader object.
                    var qrCodeReader = new ZXing.QrCode.QRCodeReader();

                    // Gets the binary bitmap of image array.
                    var binaryBitmap = DependencyService.Get<IDeviceOrientation>().GetBinaryBitmap(imageArray);

                    // Decodes the qr code form binary bitmap.
                    Result result = qrCodeReader.decode(binaryBitmap);
                    _resultString = result.Text;

                    // Shows the decoded qr code result to user and ask them whether they want to open it or not.
                    bool userResponse = await DisplayAlert("Open :", _resultString, "Open", "Cancel");

                    // If open then try opening it else go back to main page.
                    if (userResponse)
                    {
                        OpenPdf(_resultString);
                    }
                    else
                    {
                        // If user cancels removes the qr code page from stack and begin from beginning.
                        await Navigation.PopAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex is FileNotFoundException || ex is MediaFileNotFoundException)
                {
                    ActIndicator.IsRunning = false;
                    await DisplayAlert("Image not found!", "The selected image is not found", "OK");
                }
                else if (ex is NullReferenceException)
                {
                    ActIndicator.IsRunning = false;
                    await DisplayAlert("Invalid QR code", "Unable to decode the QR code", "OK");
                }
                else
                {
                    ActIndicator.IsRunning = false;
                    await DisplayAlert("A problem occured", ex.Message, "OK");
                }
            }
        }
    }
}
