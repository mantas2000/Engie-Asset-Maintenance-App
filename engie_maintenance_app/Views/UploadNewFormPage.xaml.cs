////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: UploadNewFormPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mohammed Albulushi, Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class to handle user's upload form requests.
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.WebServices;
using Plugin.FilePicker;
using Plugin.FilePicker.Abstractions;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UploadNewFormPage
    {
        private byte[] _fileData;
        private string _filePath = "";
        private string _fileName = "";
        private string _newFileName = "" ;    
        
        public UploadNewFormPage()
        {
            InitializeComponent();
            BindingContext = new GlobalSettings();
            AddPickerItems();
        }

        /// <summary>
        /// Adds form types to the picker
        /// </summary>
        private async void AddPickerItems()
        {
            List<string> formTypesList;
            try
            {
                formTypesList = await WebServices.WebServices.GetFormTypes();
            }
            catch (NoInternetException)
            {
                await DisplayAlert(NoInternetException.ErrorHeader,
                    NoInternetException.ErrorBody,
                    NoInternetException.ErrorButton);
                await Navigation.PopToRootAsync();
                return;
            }

            if (formTypesList != null)
            {
                int itemCount = formTypesList.Count;
                for (int i = 0; i < itemCount; i++)
                {
                    FormTypePicker.Items.Add(formTypesList[i]);
                }
            }
        }
        
        /// <summary>
        /// Generates QR code from given text
        /// </summary>
        /// <param name="textToGenerateQr">The text to generate the QR code from</param>
        void GenerateQrCode(string textToGenerateQr)
        {
            // If provided text is not null or empty shows the qr code else doesn't do anything.
            if (!string.IsNullOrEmpty(textToGenerateQr))
            {
                // Sets the QrImageView ( Shown in the upload page) barcode value to the text provided.
                QrImageView.BarcodeValue = textToGenerateQr;

                QrImageView.IsVisible = true;

                SaveBtn.IsVisible = true;
            }
            else
            {
                QrImageView.IsVisible = false;
                SaveBtn.IsVisible = false;
            }
        }

        /// <summary>
        /// Saves QR code to local device.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        async void SaveQrCode(object sender, EventArgs args)
        {
            var filename = _newFileName;
            if (!string.IsNullOrEmpty(filename))
            {
                // Calls the platform specific code for creating qr code.
                var picStream = DependencyService.Get<IDeviceOrientation>().
                    CreateQrCode(filename, 500, 500);

                using (var memoryStream = new MemoryStream())
                {
                    await picStream.CopyToAsync(memoryStream);
                    // Gets the bytes from stream.
                    var picBytes = memoryStream.ToArray();
                    // Calls the platform specific code to save the qr code on local device.
                    DependencyService.Get<IDeviceOrientation>().SavePictureToDevice("QR_Code_" + filename, picBytes);
                    await DisplayAlert("Success", "The QR code has been saved to your camera roll", "Okay");
                }
            }
        }

        /// <summary>
        /// Select form to upload
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        async void SelectFormClicked(object sender, EventArgs e)
        {
            try
            {
                FileData fileData = null;
                if(Device.RuntimePlatform == Device.iOS)
                {
                    // Allows only pdf types for selection.
                    string[] allowedTypes = {"com.adobe.pdf"};

                    // Opens file picker to select pdf.
                    fileData = await CrossFilePicker.Current.PickFile(allowedTypes);
                }
                else if(Device.RuntimePlatform == Device.Android)
                {
                    // Allows only pdf types for selectoin.
                    string[] allowedTypes = {"application/pdf"};

                    // Opens file picker to select pdf.
                    fileData = await CrossFilePicker.Current.PickFile(allowedTypes);
                }

                // User canceled file picking.
                if (fileData == null)
                {
                    return; 
                }
                  
                _filePath = fileData.FilePath;
                _fileName = fileData.FileName;
                _fileData = fileData.DataArray;
                  
                FilePathLabel.Text = "Selected file: " + _fileName;
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error",
                    "An unexpected error has occured. Please try again",
                    "Ok");
                Console.WriteLine("Exception choosing file: " + ex);
            } 
        }

        /// <summary>
        /// Uploads the selected PDF
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void UploadSelectedPdf(object sender, EventArgs e)
        {
            var selectedFormType = FormTypePicker.SelectedItem;
            string formName = FormName.Text;

            // Validation for the filename and file category fields are not empty.
            if (!string.IsNullOrEmpty(_fileName) && !string.IsNullOrEmpty(formName) && selectedFormType != null)
            {
                try
                {
                    // Creates a name for new file.
                    _newFileName = selectedFormType + "_" + formName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".pdf";

                    // Creates a copy of the form in the local device with correct name to upload to aws bucket.
                    string newFormPath = DependencyService.Get<IDeviceOrientation>().CopyOfForm(_fileData, _newFileName);

                    // Uploads the selected form to aws bucket
                    var test = await AwsBucket.AwsBucket.UploadFile(newFormPath, "engie-forms");

                    if (test)
                    {
                        await DisplayAlert("Success", "Your form has been uploaded to the remote server", "Okay");

                        // Generates qr code for that uploaded form
                        GenerateQrCode(_newFileName);
                    }
                    else
                    {
                        await DisplayAlert("Ooops", "Something went wrong! Please try again.", "Okay");
                    }
                }
                catch (NoInternetException)
                {
                    await DisplayAlert(NoInternetException.ErrorHeader,
                        NoInternetException.ErrorBody,
                        NoInternetException.ErrorButton);
                }
                catch (Exception exception)
                {
                    Console.WriteLine(exception);
                    await DisplayAlert("Ooops", "Something went wrong! Please try again.", "Okay");
                }
            }
            else
            {
                await DisplayAlert("You missed something!", "Make sure you selected a form and a category/name before trying to uploading form.", "okay");
            }
        }
    }
}