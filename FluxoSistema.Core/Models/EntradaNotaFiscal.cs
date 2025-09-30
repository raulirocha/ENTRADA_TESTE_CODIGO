

using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("entrada_nota_fiscal")]
    public class EntradaNotaFiscal
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("chaveacesso")]
        [StringLength(50)]
        public string? ChaveAcesso { get; set; }

        [Column("numeronota")]
        [StringLength(20)]
        public string? NumeroNota { get; set; }

        [Column("serie")]
        [StringLength(10)]
        public string?   Serie { get; set; }

        [Column("dataemissao")]
        public DateTime? DataEmissao { get; set; }

        [Column("dataentrada")]
        public DateTime? DataEntrada { get; set; }

        [Column("idfornecedor")]
        public int? IdFornecedor { get; set; }

        [Column("valortotal")]
        public decimal? ValorTotal { get; set; }

        [Column("valorprodutos")]
        public decimal? ValorProdutos { get; set; }

        [Column("baseicms")]
        public decimal? BaseIcms { get; set; }

        [Column("valoricms")]
        public decimal? ValorIcms { get; set; }

        [Column("valorfrete")]
        public decimal? ValorFrete { get; set; }

        [Column("valordesconto")]
        public decimal? ValorDesconto { get; set; }

        [Column("baseicms_st")]
        public decimal? BaseIcmsSt { get; set; }

        [Column("valoricms_st")]
        public decimal? ValorIcmsSt { get; set; }

        [Column("xmlcompleto")]
        public string? XmlCompleto { get; set; }

        [Column("datacadastro")]
        public DateTime? DataCadastro { get; set; }

        [Column("ie_entrada")]
        [StringLength(20)]
        public string? IeEntrada { get; set; }

        [Column("obs_entrada")]
        public string? ObsEntrada { get; set; }

        [Column("idempresa")]
        public int? IdEmpresa { get; set; }

        [Column("status")]
        [StringLength(50)]
        public string? Status { get; set; }

        [Column("id_perfil_fiscal")]
        public int? IdPerfilFiscal { get; set; }
    }
}