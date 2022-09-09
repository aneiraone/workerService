using Common;
using System.Text;

namespace WorkerService.Service
{
    public class WSEstadoDocumento
    {
        private static readonly HttpClient httpClient = new HttpClient();
        public static async Task<string> Invoke(string body)
        {
            string _response = string.Empty;
            using (HttpContent content = new StringContent(body, Encoding.UTF8, "application/json"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AppSettings.GetInstance().EndPoint.UrlEstadoDocumento))
            {
                request.Content = content;
                using (HttpResponseMessage response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        throw new Exception(string.Format("Received invalid HTTP response status {0}- response {1} function {2}",
                            response.StatusCode,
                            responseString,
                            System.Reflection.MethodBase.GetCurrentMethod().Name
                            ));
                    }

                    _response = await response.Content.ReadAsStringAsync();
                }
            }
            return _response;
        }
    }
}
