using Common.BL;
using Common;

namespace DbCore.Service
{
    public class EstadoService
    {
        private DocumentDbContext _context = new DocumentDbContext();

        public void Add(Documentos document, string codigo, string descripcion)
        {
            EstadosSII estadoService = new EstadosSII()
            { 
                DocumentoId = document.Id,
          //      Documento = document,
                NumeroDocumento = document.NumeroDocumento,
                TipoDocumento = document.TipoDocumento,
                RutReceptor = document.RutReceptor,
                RespuestaCodigoSII = codigo,
                RespuestaDescripSII = descripcion,
                FechaConsulta = DateTime.Now,
            };
            _context.Add(estadoService);
            _context.SaveChanges();
        }
    }
}
