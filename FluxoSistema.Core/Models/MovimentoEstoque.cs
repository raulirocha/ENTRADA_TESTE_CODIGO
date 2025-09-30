using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("movimento_estoque")]
    public class MovimentoEstoque
    {
        [Key]
        [Column("id")] // Supondo que a tabela tem uma chave primária 'id'

        public int Id { get; set; }

        [Column("id_movimento_pai")]
        public int? MovimentoPaiId { get; set; }

        [Column("id_produto")]
        public int ProdutoId { get; set; }

        [Column("id_cliente")]
        public int ClienteId { get; set; }

        [Column("id_usuario")]
        public int UsuarioId { get; set; }

        [Column("nome_produto")]
        public string? NomeProduto { get; set; }

        [Column("data_movimento")]
        public DateTime DataMovimento { get; set; }

        [Column("origem_movimento")]
        public string? OrigemMovimento { get; set; }

        [Column("tipo_movimento")]
        public string? TipoMovimento { get; set; }

        [Column("status_movimento")]
        public string? StatusMovimento { get; set; }

        [Column("id_documento")]
        public int DocumentoId { get; set; }

        [Column("id_item_documento")]
        public int ItemDocumentoId { get; set; }

        [Column("quantidade")]
        public decimal Quantidade { get; set; }

        [Column("valor_unitario")]
        public decimal ValorUnitario { get; set; }

        [Column("valor_total_bruto")]
        public decimal ValorTotalBruto { get; set; }

        [Column("valor_desconto")]
        public decimal ValorDesconto { get; set; }

        [Column("valor_total_liquido")]
        public decimal ValorTotalLiquido { get; set; }

        [Column("idempresa")]
        public int IdEmpresa { get; set; }

        [Column("id_perfil_fiscal")]
        public int PerfilFiscalId { get; set; }

        [Column("finalidade")]
        public string? Finalidade { get; set; }

        [Column("afeta_estoque_fisico")]
        public bool AfetaEstoqueFisico { get; set; }

        [Column("afeta_estoque_fiscal")]
        public bool AfetaEstoqueFiscal { get; set; }


        [Column("origem_produto")]
        public int? OrigemProduto { get; set; }


        [Column("un")]
        public string? Unidade { get; set; }

        /// <summary>
        /// O Código de Situação Tributária (CST/CSOSN) do ICMS para este item.
        /// </summary>
        [Column("cst")]
        public string? Cst { get; set; }


        [Column("ncm")]
        public string? Ncm { get; set; }

        [Column("cest")]
        public string? Cest { get; set; }

        [Column("cfop")]
        public string? Cfop { get; set; }

        [Column("cst_pis")]
        public string? CstPis { get; set; }

        [Column("base_calculo_pis")]
        public decimal? BaseCalculoPis { get; set; }

        [Column("aliquota_pis")]
        public decimal? AliquotaPis { get; set; }

        [Column("valor_pis")]
        public decimal? ValorPis { get; set; }

        [Column("cst_cofins")]
        public string? CstCofins { get; set; }

        [Column("base_calculo_cofins")]
        public decimal? BaseCalculoCofins { get; set; }

        [Column("aliquota_cofins")]
        public decimal? AliquotaCofins { get; set; }

        [Column("valor_cofins")]
        public decimal? ValorCofins { get; set; }

        // Campos para a Lei da Transparência (IBPT)
        [Column("valor_aproximado_tributos_federais")]
        public decimal? ValorAproximadoTributosFederais { get; set; }

        [Column("valor_aproximado_tributos_estaduais")]
        public decimal? ValorAproximadoTributosEstaduais { get; set; }

        [Column("valor_aproximado_tributos_municipais")]
        public decimal? ValorAproximadoTributosMunicipais { get; set; }

    }
}