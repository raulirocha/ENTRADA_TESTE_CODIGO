using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa uma linha da tabela de configurações do sistema.
    /// </summary>
    [Table("configuracao")]
    public class Configuracao
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("chave")]
        public string? Descricao { get; set; }

        [Column("valor")]
        public string? Valor { get; set; }
    }
}