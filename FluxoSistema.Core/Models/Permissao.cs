using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("permissoes")]
    public class Permissao
    {
        // A maioria das tabelas de ligação tem uma chave primária própria.
        // Se a sua não tiver, podemos ajustar, mas vamos assumir que tem por agora.
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("usuarioid")]
        public int UsuarioId { get; set; }

        [Column("permissaoid")]
        public int TipoPermissaoId { get; set; }

        // Propriedade de navegação para o EF Core fazer o JOIN
        public virtual TipoPermissao TipoPermissao { get; set; }
    }
}