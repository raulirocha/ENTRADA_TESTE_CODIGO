using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("endereco")]
    public class Endereco
    {
        [Key]
        [Column("id")] // Supondo que a tabela endereço tem um ID próprio
        public int Id { get; set; }

        [Column("clienteid")]
        public int ClienteId { get; set; }

        [Column("logradouro")]
        public string? Logradouro { get; set; }

        [Column("numero")]
        public string? Numero { get; set; }

        [Column("bairro")]
        public string? Bairro { get; set; }

        [Column("cidade")]
        public string? Cidade { get; set; }

        [Column("estado")]
        public string? Estado { get; set; }

        [Column("cep")]
        public string? Cep { get; set; }

        [Column("ccid")]
        public long CodigoMunicipio { get; set; }

        // Propriedade de navegação para o "pai" (o Cliente)
        public virtual Cliente Cliente { get; set; }
    }
}