////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: AwsBucket.cs
//FileType: Visual C# Source file
//Author : Joel Carter, Archie Vann
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Class for connecting to the aws bucket to store and download files
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System;
using System.IO;
using Amazon;
using Amazon.S3;
using Amazon.S3.Transfer;
using Amazon.CognitoIdentity;
using Amazon.Runtime;
using System.Threading.Tasks;
using engie_maintenance_app.Interfaces;
using engie_maintenance_app.Views;
using Plugin.SecureStorage;
using Xamarin.Forms;

namespace engie_maintenance_app.AwsBucket
{
    public class AwsBucket
    {
        private static readonly string _bucketName = "engie-forms";
        private static readonly RegionEndpoint _region = RegionEndpoint.EUWest2;
        private static readonly CognitoAWSCredentials _credentials = new CognitoAWSCredentials(
            "eu-west-2:46d65203-b579-47df-a8f3-aae5d2323763", _region);
        
        /// <summary>
        /// Downloads given form key to local device
        /// </summary>
        /// <param name="key">The key of the form to be downloaded</param>
        /// <returns>filePath if succeeds. "failed" if failed.</returns>
        public static string DownloadPdf(string key)
        {
            if (Network.HasInternet.HasInternetCheck())
            {
                try
                {
                    AmazonS3Client s3Client = new AmazonS3Client(_credentials, new AmazonS3Config
                    {
                        Timeout = TimeSpan.FromSeconds(1),
                        MaxErrorRetry = 2,
                        RegionEndpoint = _region
                    });

                    TransferUtility transferUtility = new TransferUtility(s3Client);
                    
                    // Creates a form history object for the form to keep history for the form.
                    string formName = key;
                    string date = DateTime.Now.ToString("MM/dd/yyyy HH:mm");
                    string status = "Not Submitted";
                    FormHistory formHistory = new FormHistory(date, formName, status);
                
                    // Create a user specific directory to keep current user files.
                    string directoryPath =  DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID"));

                    // Create a filepath for the form to download to
                    string filePath = directoryPath + "/" + key;
                    
                    // Checks if the file already exists, if so returns the filepath.
                    // else downloads the file from aws bucket and returns the filepath.

                    if (!File.Exists(filePath))
                    {
                        Console.WriteLine("filePath doesnt exist locally\n" +
                            "Trying to download filePath from AWS bucket\n" +
                            DateTime.Now);
                        try
                        {
                            transferUtility.Download(filePath, _bucketName, key);
                            DataAccessLayer.FormHistory.Add(formHistory);
                        }
                        catch (AmazonServiceException e)
                        {
                            Console.WriteLine("--------------------------");
                            Console.WriteLine(e);
                        }
                        Console.WriteLine("Download finished/timed out\n" +
                            DateTime.Now);

                        // Check download succeeded/didn't time out.
                        if (!File.Exists(filePath))
                        {
                            Console.WriteLine("filePath doesn't exist locally" +
                                "Returning \"failed\"");
                            return "failed";
                        }
                    }
                    Console.WriteLine("Returning filePath");
                    return filePath;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return "failed";
                }
            }
            else
            {
                string filePath = DependencyService.Get<IDeviceOrientation>().GetDirectory(CrossSecureStorage.Current.GetValue("ID")) + "/" + key;

                // If it exists then just return filePath.
                if (File.Exists(filePath))
                {
                    return filePath;
                }
                return "failed";
            }
        }

        /// <summary>
        /// Uploads form from given filePath to a given bucketName
        /// </summary>
        /// <param name="filePath">The local path of the file</param>
        /// <param name="bucketName">The name of the bucket to upload the form to</param>
        /// <returns>False if failed.</returns>
        public static async Task<bool> UploadFile(string filePath, string bucketName)
        {
            if (Network.HasInternet.HasInternetCheck())
            {
                try
                {
                    AmazonS3Client s3Client = new AmazonS3Client(_credentials, new AmazonS3Config
                    {
                        Timeout = TimeSpan.FromSeconds(1),
                        MaxErrorRetry = 2,
                        RegionEndpoint = _region
                    });
                    var transferUtility = new TransferUtility(s3Client);
                    transferUtility.Upload(filePath, bucketName);

                    return true;
                }
                catch (Exception e)
                {
                    throw e;
                }
            }
            else
            {               
                return false;
            }
        }
    }
}