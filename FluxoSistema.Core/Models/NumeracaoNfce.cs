using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("numeracao_nfce")] // <-- Aponta para a nova tabela
    public class NumeracaoNfce
    {
        [Key]
        [Column("id_empresa")]
        public int IdEmpresa { get; set; }

        [Column("ultimo_numero")]
        public int UltimoNumero { get; set; }
    }
}