using System.Xml.Serialization;

namespace Common.SOAP.Response
{
    [XmlRoot(ElementName = "FIND_PDOCUMENTOSResult")]
    public class ResponseDocuments
    {
        [XmlElement(ElementName = "PDOCUMENTOS")]
        public List<PDocumentos> PDocumentos { get; set; }
    }
}
