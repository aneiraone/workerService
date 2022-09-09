using Microsoft.Extensions.Configuration;

namespace Common
{
    public class Certificado
    {
        [ConfigurationKeyName("Path")]
        public string Path { get; set; }

        [ConfigurationKeyName("Pass")]
        public string Pass { get; set; }
    }
}
