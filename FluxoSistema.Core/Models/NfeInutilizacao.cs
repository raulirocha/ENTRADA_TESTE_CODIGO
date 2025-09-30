using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("nfe_inutilizacao")]
    public class NfeInutilizacao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("ano")]
        public int Ano { get; set; }

        [Column("serie")]
        public int Serie { get; set; }

        [Column("numero_inicial")]
        public int NumeroInicial { get; set; }

        [Column("numero_final")]
        public int NumeroFinal { get; set; }

        [Column("justificativa")]
        public string Justificativa { get; set; }

        [Column("data_inutilizacao")]
        public DateTime DataInutilizacao { get; set; }

        [Column("protocolo")]
        public string Protocolo { get; set; }

        [Column("xml_retorno")]
        public string XmlRetorno { get; set; }

        [Column("id_empresa")]
        public int IdEmpresa { get; set; }
    }
}