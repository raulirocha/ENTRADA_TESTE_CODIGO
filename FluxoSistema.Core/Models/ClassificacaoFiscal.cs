using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa uma classificação fiscal de um produto, como 'Tributado Integralmente' ou 'Substituição Tributária'.
    /// Esta classe mapeia a tabela 'fiscal_classificacao'.
    /// </summary>
    [Table("fiscal_classificacao")]
    public class ClassificacaoFiscal
    {
        /// <summary>
        /// A chave primária da classificação (ex: 1, 2, etc.).
        /// Corresponde à coluna 'idfiscal' na tabela de produtos.
        /// </summary>
        [Key]
        [Column("id")]
        public int Id { get; set; }

        /// <summary>
        /// A descrição da classificação fiscal (ex: "Substituição Tributária").
        /// </summary>
        [Column("descricao")]
        public string? Descricao { get; set; }

        
    }
}