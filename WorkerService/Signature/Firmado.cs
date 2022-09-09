using Common;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Text;
using System.Xml;

namespace WorkerService.Signature
{
    public class Firmado
    {
        public string Create(string seed) {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine(@"<?xml version=""1.0"" encoding=""UTF-8""?>");
            sb.AppendLine(string.Format("<getToken><item><Semilla>{0}</Semilla></item></getToken>", seed));

            var docParaFirmado = new XmlDocument{ PreserveWhitespace = true };
            docParaFirmado.LoadXml(sb.ToString());

            // Get client certificate.
            X509Certificate2 cert = new X509Certificate2(AppSettings.GetInstance().Certificado.Path, AppSettings.GetInstance().Certificado.Pass);
            SignedXml signedXml = new SignedXml(docParaFirmado.DocumentElement) { SigningKey = cert.GetRSAPrivateKey() };
            signedXml.SignedInfo.CanonicalizationMethod = "http://www.w3.org/TR/2001/REC-xml-c14n-20010315";
            signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2000/09/xmldsig#rsa-sha1";
            Reference reference = new Reference { Uri = string.Empty };
            reference.AddTransform(new XmlDsigEnvelopedSignatureTransform());
            reference.DigestMethod = "http://www.w3.org/2000/09/xmldsig#sha1";
            signedXml.AddReference(reference);
            KeyInfo keyInfo = new KeyInfo();
            keyInfo.AddClause(new KeyInfoX509Data(cert));
            signedXml.KeyInfo = keyInfo;

            // create signature
            signedXml.ComputeSignature();

            // get signature XML element and add it as a child of the root element
            signedXml.GetXml();
            docParaFirmado.DocumentElement?.AppendChild(signedXml.GetXml());
            return docParaFirmado.InnerXml;
        }
    }
}
