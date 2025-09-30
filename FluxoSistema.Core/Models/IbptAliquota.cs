using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("ibpt_aliquotas")]
    public class IbptAliquota
    {

        [Column("id")]
        public int Id { get; set; }

        [Column("uf")]
        public string Uf { get; set; }


        [Column("codigo")]
        public string Codigo { get; set; } // NCM

        [Column("ex_tipi")]
        public string ExTipi { get; set; }

        [Column("nacional_federal")]
        public decimal NacionalFederal { get; set; }

        [Column("importados_federal")]
        public decimal ImportadosFederal { get; set; }

        [Column("estadual")]
        public decimal Estadual { get; set; }


        [Column("municipal")]
        public decimal Municipal { get; set; }


        [Column("versao")]
        public string Versao { get; set; }
    }
}