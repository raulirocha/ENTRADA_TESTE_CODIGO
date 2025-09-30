using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    // Mapeamos para a tabela "usuario", usando aspas para nomes de tabelas/colunas
    // que podem ser sensíveis a maiúsculas/minúsculas no PostgreSQL.
    [Table("usuario")]
    public class Usuario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string Nome { get; set; }

        [Column("senhahash")]
        public string SenhaHash { get; set; }

        [Column("caixaid")]
        public int CaixaId { get; set; }

        [Column("empresaid")]
        public int EmpresaId { get; set; }
    }
}