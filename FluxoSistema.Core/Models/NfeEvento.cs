// Fiscal.Core/NFe/Vendas/NfeEvento.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("nfe_evento")]
    public class NfeEvento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_nota_fiscal")]
        public int NfeHeaderId { get; set; }

        [Column("tipo_evento")]
        public string TipoEvento { get; set; } // Ex: "CC-e", "Cancelamento"

        [Column("sequencia_evento")]
        public int SequenciaEvento { get; set; }

        [Column("data_evento")]
        public DateTime DataEvento { get; set; }

        [Column("justificativa_correcao")]
        public string JustificativaCorrecao { get; set; }

        [Column("xml_retorno_protocolo")]
        public string XmlRetornoProtocolo { get; set; }

        [ForeignKey("NfeHeaderId")]
        public virtual NfeHeader NfeHeader { get; set; }
    }
}