////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: PdfViewerPage.xaml.cs
//FileType: Visual C# Source file
//Author : Mantas Burcikas, Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : A class for handling PDF Viewer & Editor
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using System.Reflection;
using System.Threading.Tasks;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.Network;
using engie_maintenance_app.WebServices;
using Plugin.SecureStorage;
using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.iOSSpecific;
using Xamarin.Forms.Xaml;

namespace engie_maintenance_app.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PdfViewerPage
    {
        private bool _saveMessageValidator;
        private string _filePath;
        private string _testForSubmission = "not null";
        private FormHistory _formHistory;
        
        public PdfViewerPage(string pdfPath)
        {
            // Register Syncfusion license.
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(
                "Mzg5MDAzQDMxMzgyZTM0MmUzMFFtRkZhWlpGWnArcDgyS0VEU2lDVTJiUFM1YmlZNEY4aVdNdjN1MmVPQVk9");
            InitializeComponent();
            BindingContext = new GlobalSettings();
            On<iOS>().SetUseSafeArea(true);
            _filePath = pdfPath;
        }

        /// <summary>
        /// Save the PDF to the internal storage and get its file path.
        /// replaces the old form with the new edit pdf and deletes the new created form
        /// creates a new form history object and updates the db to contain the latest info
        /// about the form.
        /// </summary>
        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            // Disable messages displaying after PDF Viewer saves the file and closes.
            _saveMessageValidator = true;

            /*
             * Save the PDF to the internal storage and get its file path.
             * replaces the old form with the new edit pdf and deletes the new created form
             * creates a new form history object and updates the db to contain the latest info
             * about the form.
             */
            if (_testForSubmission != null)
            {
                string filePath1 = SavePDF(pdfViewerControl.SaveDocument(false) as MemoryStream);
                File.Replace(filePath1, _filePath, null);
                File.Delete(filePath1);
                var fileName = Path.GetFileName(_filePath);
                _formHistory = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), fileName, "Not Submitted");

                DataAccessLayer.FormHistory.Update(_formHistory);
            }
        }

        /// <summary>
        /// Sets up listeners and runs null checks
        /// </summary>
        protected override void OnAppearing()
        {
            base.OnAppearing();

            Stream fileStream;

            if (string.IsNullOrEmpty(_filePath))
            {
                fileStream = typeof(App).GetTypeInfo().Assembly
                    .GetManifestResourceStream("engie_maintenance_app.Forms.TEST.pdf");
            }
            else
            {
                fileStream = new FileStream(_filePath, FileMode.Open, FileAccess.Read);
            }

            // Load the PDF.
            pdfViewerControl.LoadDocument(fileStream);
            
            // Hide the 'Print' button
            pdfViewerControl.Toolbar.SetToolbarItemVisibility("print", false);


            // Check if 'Save' button is pressed.
            pdfViewerControl.DocumentSaveInitiated += PdfViewerControl_DocumentSaveInitiated;
        }

        /// <summary>
        /// Saves + emails the current document.
        /// </summary>
        /// <param name="sender">The sender has no effect on this method</param>
        /// <param name="args">Event data has no effect on this method</param>
        private async void PdfViewerControl_DocumentSaveInitiated(object sender,
            Syncfusion.SfPdfViewer.XForms.DocumentSaveInitiatedEventArgs args)
        {
            // Make sure the user wants to submit the form.
            if (await SaveButtonPressed())
            {
                try
                {
                    // Disable messages displaying after PDF Viewer saves the file and closes.
                    _saveMessageValidator = true;

                    // To not allow on disappearing code to run after saving form.
                    _testForSubmission = null;

                    // Creates a new updated pdf replaces the old one and deletes the newly created one.
                    string filePath2 = SavePDF(pdfViewerControl.SaveDocument(true) as MemoryStream);
                    File.Replace(filePath2, _filePath, null);
                    File.Delete(filePath2);

                    // Forms details.
                    var fileName = Path.GetFileName(_filePath);

                    // Initializes the FormHistory object.
                    FormHistory form;

                    // Test for if the form submission failed or not.
                    bool formSubmittedSuccess = false;

                    // Email the PDF.
                    if (HasInternet.HasInternetCheck())
                    {
                        formSubmittedSuccess = await EmailService.Email.SendForm(_filePath);
                    }
                    else
                    {
                        throw new NoInternetException();
                    }

                    if (formSubmittedSuccess)
                    {
                        // Creates a new FormHistory object and set the formSubmittedSuccess to true (Success).
                        form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), fileName, "Success");
                    }
                    else
                    {
                        // Creates a new FormHistory object and set the formSubmittedSuccess to false (Failed).
                        form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), fileName, "Failed");
                    }

                    // Updates the formHistory status.
                    await DataAccessLayer.FormHistory.Update(form);

                    // Create 'Failed' message stating that submission is failed.
                    string message =
                        "Failed! The form has not been submitted, please connect to the internet and resubmit the form from the queue page";

                    // Unload the PDF document from the PDF Viewer, freeing all the accessed resources.
                    pdfViewerControl.Unload();

                    // Dispose all the managed resources PDF Viewer.
                    pdfViewerControl.Dispose();

                    if (formSubmittedSuccess)
                    {
                        // Deletes the downloaded file.
                        File.Delete(_filePath);

                        // If submission is successful changes message to success.
                        await DisplayAlert("Success:", "The form has been successfully submitted", "OK");

                        // Exits the pdf
                        await Navigation.PopToRootAsync();
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
                catch (NoInternetException)
                {
                    await DisplayAlert(NoInternetException.ErrorHeader,
                        NoInternetException.ErrorBody,
                        NoInternetException.ErrorButton);

                    FormHistory form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), Path.GetFileName(_filePath), "Failed");
                    await DataAccessLayer.FormHistory.Update(form);

                    // Exits the pdf
                    await Navigation.PopToRootAsync();
                }
                catch (Exception)
                {
                    await DisplayAlert("Error",
                        "An unexpected error has occured. Please try resubmitting from the queue page.",
                        "Ok");

                    FormHistory form = new FormHistory(DateTime.Now.ToString("MM/dd/yyyy HH:mm"), Path.GetFileName(_filePath), "Failed");
                    await DataAccessLayer.FormHistory.Update(form);

                    // Exits the pdf
                    await Navigation.PopToRootAsync();
                }
            }
        }


        /// <summary>
        /// Get confirmation from the user that they are about to submit the form.
        /// </summary>
        /// <returns>True if the answer is not known</returns>
        private async Task<bool> SaveButtonPressed()
        {
            if (_saveMessageValidator == false)
            {
                // Get confirmation from the user about submission of the form.
                return await DisplayAlert("", "Are you sure you want to submit this form?", "Yes", "No");
            }
            // Return false if the answer is already known.
            return false;
        }

        /// <summary>
        /// Saves the PDF locally.
        /// </summary>
        /// <param name="fileStream">The file to save</param>
        /// <returns>The file path of the created PDF.</returns>
        private string SavePDF(MemoryStream fileStream)
        {
            // Get current user directory path.
            string path = DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));

            // Create PDF's name that contains current date and time combined together.
            string fileName = DateTime.Now.ToString("yyyyMMddHHmmss") + ".pdf";

            // Get PDF file path.
            string filepath = Path.Combine(path, fileName);

            // Create PDF file.
            FileStream outputFileStream = File.Open(filepath, FileMode.Create);
            fileStream.Position = 0;
            fileStream.CopyTo(outputFileStream);
            outputFileStream.Close();

            // Return file path of created PDF file.
            return filepath;
        }
    }
}