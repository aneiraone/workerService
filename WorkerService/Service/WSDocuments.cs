using Common;
using Common.SOAP;
using Common.SOAP.Request;
using Common.SOAP.Response;
using System.Text;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace WorkerService.Service
{
    public class WSDocuments
    {

        private static string Node = "FIND_PDOCUMENTOSResult";
        public static async Task<ResponseDocuments> FindDocumentos()
        {
            HttpClient httpClient = new HttpClient();
            Utils utils = new Utils();
            int fInicio = 0;
            int fFinal = 0;
            if (AppSettings.GetInstance().FechaPersonalizada.ActivaFecha)
            {
                fInicio = Convert.ToInt32(AppSettings.GetInstance().FechaPersonalizada.FechaDesde);
                fFinal = Convert.ToInt32(AppSettings.GetInstance().FechaPersonalizada.FechaHasta);
            }
            else
            {
                fInicio = Convert.ToInt32(DateTime.Now.AddDays(-1).ToString("yyyyMMdd"));
                fFinal = fInicio;
            }

            string rq = new RequestDocuments().Get(1, fInicio, fFinal);

            ResponseDocuments responseDocuments = new ResponseDocuments();
            using (HttpContent content = new StringContent(rq, Encoding.UTF8, "text/xml"))
            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, AppSettings.GetInstance().EndPoint.UrlDocuments))
            {
                //request.Headers.Add("SOAPAction", "");
                request.Content = content;
                httpClient.Timeout = TimeSpan.FromSeconds(60);
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
                    XmlSerializer serializer = new XmlSerializer(typeof(ResponseDocuments));
                    foreach (var element in doc.Descendants())
                    {
                        element.Attributes().Where(a => a.IsNamespaceDeclaration).Remove();
                        element.Name = element.Name.LocalName;
                        if (element.Name.LocalName == "rut_cliente" && element.Value == "0")
                        {
                            element.Value = AppSettings.GetInstance().RutDefault;
                        }
                        else if (element.Name.LocalName == "rut_cliente" && element.Value != "0")
                        {
                            element.Value = utils.getRut(element.Value);
                        }
                        else if (element.Name.LocalName == "tipo_documento_cobro")
                        {
                            element.Value = TipoDocumento.Get(element.Value);
                        }

                    }
                    XDocument pDocumentosResult = new XDocument((from xml2 in doc.Descendants(Node) select xml2).ToList());
                    if (pDocumentosResult.Root == null) { responseDocuments.PDocumentos = new List<PDocumentos>(); return responseDocuments; }
                    responseDocuments = (ResponseDocuments)serializer.Deserialize(new StringReader(pDocumentosResult.ToString()));
                }
            }
            return responseDocuments;
        }
    }
}
