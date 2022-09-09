using Microsoft.Extensions.Configuration;

namespace Common
{
    public class EndPoints
    {
        [ConfigurationKeyName("EndpointsDocuments")]
        public string UrlDocuments { get; set; }

        [ConfigurationKeyName("EndpointsSIISeed")]
        public string UrlSemilla { get; set; }

        [ConfigurationKeyName("EndpointsSIIToken")]
        public string UrlToken { get; set; }

        [ConfigurationKeyName("EndpointsSIIEstado")]
        public string UrlEstado { get; set; }

        [ConfigurationKeyName("EndpointsEstadoDocumento")]
        public string UrlEstadoDocumento { get; set; }
    }
}
