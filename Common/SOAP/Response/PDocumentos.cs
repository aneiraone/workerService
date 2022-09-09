using System.Xml.Serialization;

namespace Common.SOAP.Response
{
	[XmlRoot(ElementName = "PDOCUMENTOS")]
	public class PDocumentos
	{
		[XmlElement(ElementName = "tipo_documento_cobro")]
		public int TipoDocumentoCobro { get; set; }

		[XmlElement(ElementName = "numero_documento_cobro")]
		public int NumeroDocumentoCobro { get; set; }

		[XmlElement(ElementName = "fecha_emision")]
		public int FechaEmision { get; set; }

		[XmlElement(ElementName = "valor_neto")]
		public int ValorNeto { get; set; }

		[XmlElement(ElementName = "valor_iva")]
		public int ValorIva { get; set; }

		[XmlElement(ElementName = "saldo_impago")]
		public int SaldoImpago { get; set; }

		[XmlElement(ElementName = "rut_cliente")]
		public string RutCliente { get; set; }
	}
}
