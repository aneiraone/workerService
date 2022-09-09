using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Common.BL
{
    [Table("estados_sii")]
    public class EstadosSII
    {
        [Key]
        public int Id { get; set; }

        public int DocumentoId { get; set; }
        [JsonIgnore]
      //  [IgnoreDataMember]
        public virtual Documentos Documento { get; set; }

        [Column("rut_receptor"), MaxLength(11), ForeignKey("rut_receptor")]
        public string RutReceptor { get; set; }

        [Column("tipo_documento"), ForeignKey("tipo_documento")]
        public int TipoDocumento { get; set; }

        [Column("numero_documento"), ForeignKey("numero_documento")]
        public int NumeroDocumento { get; set; }

        [Column("fecha_consulta")]
        public DateTime FechaConsulta { get; set; }

        [Column("respuesta_codigo_sii"), MaxLength(15)]
        public string RespuestaCodigoSII { get; set; }

        [Column("respuesta_descrip_sii"), MaxLength(250)]
        public string RespuestaDescripSII { get; set; }

    }
}
