using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa uma regra fiscal específica dentro de um Perfil Fiscal.
    /// Mapeia a tabela 'fiscal_regra_fiscal'.
    /// </summary>
    [Table("fiscal_regra_fiscal")]
    public class RegraFiscal
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idperfilfiscal")]
        public int IdPerfilFiscal { get; set; }

        [Column("idempresa")]
        public int? IdEmpresa { get; set; }

        [Column("idclassificacaofiscal")]
        public int? IdClassificacaoFiscal { get; set; }

        [Column("ufdestino")]
        public string? UfDestino { get; set; }

        [Column("cfopdentro")]
        public string? CfopDentroEstado { get; set; }

        [Column("cfopfora")]
        public string? CfopForaEstado { get; set; }

        [Column("cst")]
        public string? Cst { get; set; }

        [Column("cstpis")]
        public string? CstPis { get; set; }

        [Column("cstcofins")]
        public string? CstCofins { get; set; }

        [Column("cest")]
        public string? Cest { get; set; }

        [Column("aliquotaicms")]
        public decimal? AliquotaIcms { get; set; }

        [Column("aliquotapis")]
        public decimal? AliquotaPis { get; set; }

        [Column("aliquotacofins")]
        public decimal? AliquotaCofins { get; set; }

        [Column("aliquotafcp")]
        public decimal? AliquotaFcp { get; set; }

        [Column("aliquotafcpst")]
        public decimal? AliquotaFcpst { get; set; }

        [Column("origemproduto")]
        public int? OrigemProduto { get; set; }

        [Column("modalidadebc")]
        public int? ModalidadeBc { get; set; }

        [Column("modalidadebcst")]
        public int? ModalidadeBcst { get; set; }

        [Column("reducaobc")]
        public decimal? ReducaoBc { get; set; }

        [Column("mvast")]
        public decimal? MvaSt { get; set; }

        [Column("baseicms")]
        public decimal? BaseIcms { get; set; }

        [Column("baseicms_st")]
        public decimal? BaseIcmsSt { get; set; }

        [Column("codigoenquadramentoipi")]
        public string? CodigoEnquadramentoIpi { get; set; }

        [Column("incluinototal")]
        public bool IncluiNoTotal { get; set; }

        [Column("natop")]
        public string? NaturezaOperacaoFiscal { get; set; }

        [Column("obs")]
        public string? Observacao { get; set; }

        [Column("datacriacao")]
        public DateTime DataCriacao { get; set; }

        // Campos que existem no banco mas não parecem estar no DTO principal ainda.
        // É bom tê-los mapeados para o futuro.
        [Column("naturezareceitacodigo")]
        public string? NaturezaReceitaCodigo { get; set; }

        [Column("issubstituicaotributaria")]
        public bool? IsSubstituicaoTributaria { get; set; }

        [Column("cst_destino")]
        public string? CstDestino { get; set; }

        /// <summary>
        /// Propriedade de navegação para o Perfil Fiscal.
        /// </summary>
        [ForeignKey("IdPerfilFiscal")]
        public virtual PerfilFiscal PerfilFiscal { get; set; }
    }
}