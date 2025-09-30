// Local: FluxoSistema.Core/Models/ProdutoEstoqueSaldo.cs
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("produto_estoque_saldo")]
    public class ProdutoEstoqueSaldo
    {
        [Key]
        [Column("id_produto")]
        public int ProdutoId { get; set; }

        [Column("saldo_fisico")]
        public decimal SaldoFisico { get; set; }

        [Column("saldo_fiscal")]
        public decimal SaldoFiscal { get; set; }

        [Column("data_ultima_atualizacao")]
        public DateTime DataUltimaAtualizacao { get; set; }

        // Propriedade de navegação para facilitar as consultas com Entity Framework
        public virtual Produto Produto { get; set; }
    }
}