using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("numeracao_nfe")]
    public class NumeracaoNfe
    {
        [Key]
        [Column("id_empresa")]
        public int IdEmpresa { get; set; }

        [Column("ultimo_numero")]
        public int UltimoNumero { get; set; }
    }
}