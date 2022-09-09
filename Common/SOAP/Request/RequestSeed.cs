using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.SOAP.Request
{
    public class RequestSeed
    {
        public string Get(int tipoDocumento, int fechaInicio, int fechaFin)
        {
            return string.Format(@"<soap:Envelope xmlns:soap=""http://www.w3.org/2003/05/soap-envelope"" xmlns:tem=""http://tempuri.org/"">
                   <soap:Header/>
                   <soap:Body>
                      <tem:FIND_PDOCUMENTOS>
                         <tem:V_TIPO_DOCUMENTO>1</tem:V_TIPO_DOCUMENTO>
                         <tem:V_FECHA_INICIO>20220805</tem:V_FECHA_INICIO>
                         <tem:V_FECHA_FIN>20220805</tem:V_FECHA_FIN>
                      </tem:FIND_PDOCUMENTOS>
                   </soap:Body>
                </soap:Envelope>", tipoDocumento, fechaInicio, fechaFin);
        }
    }
}
