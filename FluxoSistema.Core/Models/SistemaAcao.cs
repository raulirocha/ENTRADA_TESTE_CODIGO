using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("sistema_acoes")]
    public class SistemaAcao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("chave_acao")]
        public string ChaveAcao { get; set; }
    }
}