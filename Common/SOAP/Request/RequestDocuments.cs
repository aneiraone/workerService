using System.Xml.Serialization;

namespace Common.SOAP.Request
{
    public class RequestDocuments
    {
        public string Get(int tipoDocumento, int fechaInicio, int fechaFin) {
            return string.Format(@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:tem=""http://tempuri.org/"">
                   <soap:Header/>
                   <soap:Body>
                      <tem:FIND_PDOCUMENTOS>
                         <tem:V_TIPO_DOCUMENTO>{0}</tem:V_TIPO_DOCUMENTO>
                         <tem:V_FECHA_INICIO>{1}</tem:V_FECHA_INICIO>
                         <tem:V_FECHA_FIN>{2}</tem:V_FECHA_FIN>
                      </tem:FIND_PDOCUMENTOS>
                   </soap:Body>
                </soap:Envelope>", tipoDocumento, fechaInicio, fechaFin);
        }

        [XmlRoot(ElementName = "FIND_PDOCUMENTOS")]
        public class FINDPDOCUMENTOS
        {

            [XmlElement(ElementName = "V_TIPO_DOCUMENTO")]
            public int VTIPODOCUMENTO { get; set; }

            [XmlElement(ElementName = "V_FECHA_INICIO")]
            public int VFECHAINICIO { get; set; }

            [XmlElement(ElementName = "V_FECHA_FIN")]
            public int VFECHAFIN { get; set; }
        }

        [XmlRoot(ElementName = "Body")]
        public class Body
        {

            [XmlElement(ElementName = "FIND_PDOCUMENTOS")]
            public FINDPDOCUMENTOS FINDPDOCUMENTOS { get; set; }
        }

        [XmlRoot(ElementName = "Envelope")]
        public class Envelope
        {
            [XmlElement(ElementName = "Body")]
            public Body Body { get; set; }
        }
    }
}
