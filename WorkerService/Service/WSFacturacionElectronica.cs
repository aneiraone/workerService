using Common;
using Newtonsoft.Json.Linq;
using System.Net;
using System.Text;
using System.Xml.Linq;
using WorkerService.Signature;

namespace WorkerService.Service
{
    public class WSFacturacionElectronica
    {
        //private static readonly HttpClient httpClient = new HttpClient();
        private async Task<string> GetSeed()
        {
            HttpClient httpClient = new HttpClient();
            string seed = string.Empty;
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AppSettings.GetInstance().EndPoint.UrlSemilla))
            {
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
                    //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                    XDocument doc = XDocument.Parse(await response.Content.ReadAsStringAsync());
                    foreach (var element in doc.Descendants())
                    {
                        element.Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
                        element.Name = element.Name.LocalName;
                    }
                    seed = doc.Descendants().Where(a => a.Name.LocalName == "SEMILLA").FirstOrDefault().Value;
                }
            }
            return seed;
        }

        private async Task<string> GetToken(string body)
        {
            HttpClient httpClient = new HttpClient();
            string token = string.Empty;
            using (HttpContent content = new StringContent(body, Encoding.UTF8, "application/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AppSettings.GetInstance().EndPoint.UrlToken))
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
                    //response.EnsureSuccessStatusCode(); // throws an Exception if 404, 500, etc.
                    XDocument doc = XDocument.Parse(await response.Content.ReadAsStringAsync());
                    foreach (var element in doc.Descendants())
                    {
                        element.Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
                        element.Name = element.Name.LocalName;
                    }
                    token = doc.Descendants().Where(a => a.Name.LocalName == "TOKEN").FirstOrDefault().Value;
                }
            }
            return string.Format("TOKEN={0}", token);
        }

        public async Task<string> GetEstado(string token, string rut, int folio, int tipoDocumento, int rutReceptor, string digitoReceptor,
            int monto, DateTime fechaEmision)
        {
           // HttpClient httpClient = new HttpClient();
            UriBuilder url = new UriBuilder(new Uri(AppSettings.GetInstance().EndPoint.UrlEstado).AbsoluteUri + string.Format("{0}-{1}-{2}/estado", rut, tipoDocumento, folio));
            url.Query = string.Format("rut_receptor={0}&dv_receptor={1}&monto={2}&fechaEmision={3}", rutReceptor,
                digitoReceptor, monto, fechaEmision.ToString("dd-MM-yyyy"));

            var baseAddress = new Uri(url.Uri.AbsoluteUri);
            HttpRequestMessage requestMessage = new HttpRequestMessage { Method = HttpMethod.Get };
            var cookieContainer = new CookieContainer();
            using (var handler = new HttpClientHandler() { CookieContainer = cookieContainer })
            {
                using (HttpClient client = new HttpClient(handler) { BaseAddress = baseAddress })
                {
                    cookieContainer.Add(baseAddress, new Cookie("Cookie", token));
                    var response = await client.GetAsync(baseAddress, HttpCompletionOption.ResponseHeadersRead);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                    {
                        string responseString = await response.Content.ReadAsStringAsync();
                        throw new Exception(string.Format("Received invalid HTTP response status {0}- response {1} function {2}",
                            response.StatusCode,
                            responseString,
                            System.Reflection.MethodBase.GetCurrentMethod().Name
                            ));
                    }
                    return await response.Content.ReadAsStringAsync();

                }
            }
        }

        public async Task<Token> GetToken(double expired)
        {
            string seed = await GetSeed();
            string body = new Firmado().Create(seed);
            string _token = await GetToken(body);
            return new Token(_token, expired);
        }
    }
}