using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("numeracao_produto")]
    public class NumeracaoProduto
    {
        [Column("sequencia")]
        public int Sequencia { get; set; }
    }
}