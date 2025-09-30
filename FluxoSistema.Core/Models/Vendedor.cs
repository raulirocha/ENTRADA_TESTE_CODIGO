using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa os dados de um vendedor no sistema.
    /// </summary>
    [Table("vendedores")] // Diz ao EF Core o nome exato da tabela
    public class Vendedor
    {
        [Key] // Marca 'Id' como a chave primária
        [Column("id")]
        public int Id { get; set; }

        [Column("nome")]
        public string? Nome { get; set; }

        [Column("cpf_cnpj")]
        public string? CpfCnpj { get; set; }

        [Column("ativo")]
        public bool Ativo { get; set; }
    }
}