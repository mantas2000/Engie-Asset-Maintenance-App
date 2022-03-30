////////////////////////////////////////////////////////////////////////////////////////////////////////
//FileName: HttpRequest.cs
//FileType: Visual C# Source file
//Author : Joel Carter
//Copy Rights : Velocity Solutions Ltd (Team 24)
//Description : Write here what the class is for
////////////////////////////////////////////////////////////////////////////////////////////////////////
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace engie_maintenance_app.DataAccessLayer
{
    public class HttpRequest
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> PostHttpRequest(IEnumerable<KeyValuePair<string, string>> parameters, string url)
        {
            HttpContent urlEncodedContent = new FormUrlEncodedContent(parameters);
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "VGVzdFVzZXI6VGVzdDEyMzQh");
                using (HttpResponseMessage response = await client.PostAsync(url, urlEncodedContent))
                {
                    using (HttpContent content = response.Content)
                    {
                        string contentStr = await content.ReadAsStringAsync();
                        
                        return contentStr;
                    }
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<string> GetHttpRequest(string url)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "VGVzdFVzZXI6VGVzdDEyMzQh");
                using (HttpResponseMessage response = await client.GetAsync(url))
                {
                    using (HttpContent content = response.Content)
                    {
                        string contentStr = await content.ReadAsStringAsync();

                        return contentStr;
                    }
                }
            }
        }
    }
}