using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("tipos_permissao")]
    public class TipoPermissao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // A coluna no seu SQL é 'nome', vamos mapeá-la.
        [Column("nome")]
        public string ChavePermissao { get; set; }
    }
}