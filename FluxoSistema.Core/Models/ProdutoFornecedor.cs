using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("produto_fornecedor")]
    public class ProdutoFornecedor
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idproduto")]
        public int IdProduto { get; set; }

        [Column("idfornecedor")]
        public int IdFornecedor { get; set; }

        [Column("codigofornecedor")]
        [MaxLength(50)]
        public string? CodigoFornecedor { get; set; }

        [Column("descricaofornecedor")]
        [MaxLength(150)]
        public string? DescricaoFornecedor { get; set; }

        [Column("unidade_fornecedor")]
        [MaxLength(10)]
        public string? UnidadeFornecedor { get; set; }

        [Column("fator_conversao_entrada")]
        public decimal? FatorConversaoEntrada { get; set; }

        [Column("ultimo_custo_compra")]
        public decimal? UltimoCustoCompra { get; set; }

        [Column("data_ultima_compra")]
        public DateTime? DataUltimaCompra { get; set; }
    }
}
