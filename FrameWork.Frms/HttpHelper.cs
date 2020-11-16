using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace FrameWork.Frms
{
    public class HttpHelper
    {
        public static async Task<string> HttpPost(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Post;

            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty;
        }

        public static async Task<string> HttpGet(string url)
        {
            HttpRequestMessage request = new HttpRequestMessage();
            request.RequestUri = new Uri(url);
            request.Method = HttpMethod.Get;

            HttpClient client = new HttpClient();
            var response = await client.SendAsync(request);
            return response.IsSuccessStatusCode ? response.Content.ReadAsStringAsync().Result : string.Empty;
        }

  
    }
}
