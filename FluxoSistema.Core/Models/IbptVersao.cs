using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("ibpt_versoes")]
    public class IbptVersao
    {
        [Column("id")]
        public int Id { get; set; }

        [Column("uf")]
        public string Uf { get; set; }

        [Column("versao")]
        public string Versao { get; set; }

        [Column("chave_ibpt")]
        public string ChaveIbpt { get; set; }

        [Column("fonte")]
        public string Fonte { get; set; }

        [Column("vigencia_inicio")]
        public DateTime VigenciaInicio { get; set; }

        [Column("vigencia_fim")]
        public DateTime VigenciaFim { get; set; }

        [Column("data_importacao")]
        public DateTime DataImportacao { get; set; }
    }
}