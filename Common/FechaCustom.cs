using Microsoft.Extensions.Configuration;

namespace Common
{
    public class FechaCustom
    {
        [ConfigurationKeyName("Activa")]
        public bool ActivaFecha { get; set; }

        [ConfigurationKeyName("FechaDesde")]
        public string FechaDesde { get; set; }

        [ConfigurationKeyName("FechaHasta")]
        public string FechaHasta { get; set; }
    }
}
