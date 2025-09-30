using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("produto")]
    public class Produto
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("tipo")]
        [StringLength(20)]
        public string? Tipo { get; set; }

        [Column("codigoproduto")]
        [StringLength(50)]
        public string? CodigoProduto { get; set; }

        [Column("referencia")]
        [StringLength(50)]
        public string? Referencia { get; set; }

        [Column("descricao_prod")]
        [StringLength(150)]
        public string? Descricao_PROD { get; set; }

        [Column("codigobarra")]
        [StringLength(50)]
        public string? CodigoBarras { get; set; }

        [Column("unidade")]
        [StringLength(10)]
        public string? Unidade { get; set; }

        [Column("ncm")]
        [StringLength(10)]
        public string? NCM { get; set; }

        [Column("precofabrica")]
        public decimal? PrecoFabrica { get; set; }

        [Column("precocusto")]
        public decimal? PrecoCusto { get; set; }

        [Column("precovenda")]
        public decimal? PrecoVenda { get; set; }

        [Column("precominimo")]
        public decimal? PrecoMinimo { get; set; }

        [Column("precovista")]
        public decimal? PrecoVista { get; set; }

        [Column("margemlucro")]
        public decimal? MargemLucro { get; set; }

        [Column("estoqueatual")]
        public decimal? EstoqueAtual { get; set; }

        [Column("estoquefiscal")]
        public decimal? EstoqueFiscal { get; set; }

        [Column("estoqueminimo")]
        public decimal? EstoqueMinimo { get; set; }

        [Column("estoquemaximo")]
        public decimal? EstoqueMaximo { get; set; }

        [Column("estoquegalpao")]
        public decimal? EstoqueGalpao { get; set; }

        [Column("localizacao")]
        [StringLength(50)]
        public string? Localizacao { get; set; }

        [Column("categoriaid")]
        public int? CategoriaId { get; set; }

        [Column("subcategoriaid")]
        public int? SubcategoriaId { get; set; }

        [Column("fornecedorid")]
        public int? FornecedorId { get; set; }

        [Column("centrocustoid")]
        public int? CentroCustoId { get; set; }

        [Column("datacadastro")]
        public DateTime? DataCadastro { get; set; }

        [Column("dataultimacompra")]
        public DateTime? DataUltimaCompra { get; set; }

        [Column("dataultimavenda")]
        public DateTime? DataUltimaVenda { get; set; }

        [Column("ativo")]
        public bool? Ativo { get; set; }

        [Column("observacoes")]
        public string? Observacoes { get; set; }

        [Column("marcaid")]
        public int? MarcaId { get; set; } // Renomeado para IdMarca para consistência

        [Column("cst")]
        [StringLength(4)]
        public string? Cst { get; set; } // Renomeado para Csosn para maior clareza (CST/CSOSN)

        [Column("cest")]
        [StringLength(7)]
        public string? Cest { get; set; }

        [Column("origem")]
        public int? Origem { get; set; } // Alterado para string para corresponder ao ViewModel

        [Column("idfiscal")]
        public int? IdFiscal { get; set; }

        // Propriedade de navegação para a Marca (relacionamento)
        public virtual Marca? Marca { get; set; }
    }
}