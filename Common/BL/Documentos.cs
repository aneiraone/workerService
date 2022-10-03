using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.BL
{
  //  [JsonObject(IsReference = true)]
    [Table("documentos")]
    public class Documentos
    {
        //private readonly ILazyLoader _lazyLoader;
        //public Documentos(ILazyLoader lazyLoader)
        //{
        //    _lazyLoader = lazyLoader;
        //}
        //public Documentos()
        //{
        //}
 
        [Key]
        public int Id { get; set; }

        [Column("rut_emisor"), MaxLength(11), ForeignKey("rut_emisor")]
        public string RutEmisor { get; set; }

        [Column("nombre_empresa"), MaxLength(500)]
        public string NombreEmpresa { get; set; }

        [Column("rut_receptor"), MaxLength(11), ForeignKey("rut_receptor")]
        public string RutReceptor { get; set; }

        [Column("tipo_documento"), ForeignKey("tipo_documento")]
        public int TipoDocumento { get; set; }

        [Column("numero_documento"), ForeignKey("numero_documento")]
        public int NumeroDocumento { get; set; }

        [Column("fecha_emision")]
        public DateTime FechaEmision { get; set; }

        [Column("valor_neto")]
        public int ValorNeto { get; set; }

        [Column("valor_iva")]
        public int ValorIVA { get; set; }

        [Column("total")]
        public int Total { get; set; }

        [Column("estado1"), MaxLength(500)]
        public string Estado1 { get; set; }

        [Column("estado2"), MaxLength(500)]
        public string Estado2 { get; set; }
        public DateTime FechaCreacion { get; set; }

        [Column("Enviado")]
        public bool Enviado { get; set; }

        //private List<EstadosSII> _lineas;
        //public List<EstadosSII> Lineas
        //{
        //    get => _lazyLoader.Load(this, ref _lineas);
        //    set => _lineas = value;
        //[JsonIgnore]
        //[IgnoreDataMember]
        public virtual ICollection<EstadosSII> LineasEstado { get; set; }
    }
}
