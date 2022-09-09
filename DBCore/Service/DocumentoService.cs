using Common.BL;
using Common;
using Microsoft.EntityFrameworkCore;

namespace DbCore.Service
{
    public class DocumentoService
    {
        private string rutNull = "No se encontro el campo Rut";
        private string nombreNull = "No se encontro el campo NombreEmpresa";
        private DocumentDbContext _context = new DocumentDbContext();

        public void Add(string rutReceptor, int tipo, DateTime fechaEmision, int folio, int total, int valorIVA, int valorNeto)
        {
            if (AppSettings.GetInstance().Rut == null) { throw new NullReferenceException(rutNull); }
            if (AppSettings.GetInstance().NombreEmpresa == null) { throw new NullReferenceException(nombreNull); }
            int exits = _context.Documento.Count(x => x.RutEmisor == AppSettings.GetInstance().Rut && x.NumeroDocumento == folio && x.TipoDocumento == tipo);
            if (exits > 0) { return; }

            Documentos documento = new Documentos()
            {
                RutEmisor = AppSettings.GetInstance().Rut,
                RutReceptor = rutReceptor,
                NombreEmpresa = AppSettings.GetInstance().NombreEmpresa,
                TipoDocumento = tipo,
                FechaEmision = fechaEmision,
                NumeroDocumento = folio,
                Total = total,
                ValorIVA = valorIVA,
                ValorNeto = valorNeto,
                Estado1 = string.Empty,
                Estado2 = string.Empty,
                FechaCreacion = DateTime.Now

            };
            _context.Add(documento);
            _context.SaveChanges();
        }

        public bool Update(string rutReceptor, int tipo, DateTime fechaEmision, int folio, int total, int valorIVA, int valorNeto)
        {
            if (AppSettings.GetInstance().Rut == null) { throw new NullReferenceException(rutNull); }
            Documentos documento = _context.Documento.First(x => x.NumeroDocumento == folio && x.TipoDocumento == tipo && x.RutEmisor == AppSettings.GetInstance().Rut);
            documento.RutReceptor = rutReceptor;
            documento.TipoDocumento = tipo;
            documento.FechaEmision = fechaEmision;
            documento.NumeroDocumento = folio;
            documento.Total = total;
            documento.ValorIVA = valorIVA;
            documento.ValorNeto = valorNeto;
            _context.SaveChanges();
            return true;
        }

        public bool Update(int Id, string mensaje)
        {
            Documentos documento = _context.Documento.First(x => x.Id == Id);
            if (documento.Estado1.Length == 0)
            {
                documento.Estado1 = mensaje;
            }
            else
            {
                documento.Estado2 = documento.Estado1;
                documento.Estado1 = mensaje;
            }
            _context.SaveChanges();
            return true;
        }

        public List<Documentos> GetDocuments()
        {
            DateTime fecha = DateTime.Now.AddDays(AppSettings.GetInstance().MaxDate * -1);
            return _context.Documento.Where(d => d.FechaCreacion >= fecha).OrderBy(d => d.Estado1).ToList();
        }

        public List<Documentos> GetDocumentsWithStatus()
        {
            DateTime fecha = DateTime.Now.AddDays(AppSettings.GetInstance().MaxDate * -1);
            List<Documentos> documentos = _context.Documento.
                Include(b=> b.LineasEstado).
                Where(d => d.FechaCreacion >= fecha && d.Estado1 != string.Empty && d.Estado1 != d.Estado2).
                OrderByDescending(d => d.FechaCreacion).ToList();
            return documentos;
        }
    }
}
