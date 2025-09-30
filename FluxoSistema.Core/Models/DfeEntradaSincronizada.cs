// Salve como: FluxoSistema.Core/Models/DfeEntradaSincronizada.cs
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("dfe_entrada_sincronizada")]
    public class DfeEntradaSincronizada
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("chaveacesso")]
        public string? ChaveAcesso { get; set; }

        [Column("numeronota")]
        public string? NumeroNota { get; set; }

        [Column("serie")]
        public string? Serie { get; set; }

        [Column("modelo")]
        public string? Modelo { get; set; }

        [Column("tipooperacao")]
        public char? TipoOperacao { get; set; }

        [Column("tipoemissao")]
        public string? TipoEmissao { get; set; }

        [Column("naturezaoperacao")]
        public string? NaturezaOperacao { get; set; }

        [Column("cnpjemitente")]
        public string? CnpjEmitente { get; set; }

        [Column("nomeemitente")]
        public string? NomeEmitente { get; set; }

        [Column("ufemitente")]
        public string? UfEmitente { get; set; }

        [Column("municipioemitente")]
        public string? MunicipioEmitente { get; set; }

        [Column("cnpjdestinatario")]
        public string? CnpjDestinatario { get; set; }

        [Column("nomedestinatario")]
        public string? NomeDestinatario { get; set; }

        [Column("dataemissao")]
        public DateTime? DataEmissao { get; set; }

        [Column("datarecebimento")]
        public DateTime? DataRecebimento { get; set; }

        [Column("valorprodutos")]
        public decimal? ValorProdutos { get; set; }

        [Column("valoricms")]
        public decimal? ValorIcms { get; set; }

        [Column("valoripi")]
        public decimal? ValorIpi { get; set; }

        [Column("valortotal")]
        public decimal? ValorTotal { get; set; }

        [Column("tipoimportacao")]
        public string? TipoImportacao { get; set; }

        [Column("statusimportacao")]
        public string? StatusImportacao { get; set; }

        [Column("manifestostatus")]
        public string? ManifestoStatus { get; set; }

        [Column("manifestadoem")]
        public DateTime? ManifestadoEm { get; set; }

        [Column("xmlbaixadoem")]
        public DateTime? XmlBaixadoEm { get; set; }

        [Column("xmlcompleto")]
        public string? XmlCompleto { get; set; }

        [Column("finalidadeemissao")]
        public char? FinalidadeEmissao { get; set; }

        [Column("presencacomprador")]
        public char? PresencaComprador { get; set; }

        [Column("formapagamento")]
        public char? FormaPagamento { get; set; }

        [Column("numeroprotocolo")]
        public string? NumeroProtocolo { get; set; }

        [Column("statussefaz")]
        public string? StatusSefaz { get; set; }

        [Column("datamovimento")]
        public DateTime? DataMovimento { get; set; }

        [Column("nsu")]
        public string? Nsu { get; set; }

        [Column("idempresa")]
        public int? IdEmpresa { get; set; }

        [Column("valorbaseicms")]
        public decimal? ValorBaseIcms { get; set; }

        // Propriedade de Navegação: Uma nota pode ter vários itens.
        public virtual ICollection<DfeItemSincronizado> Itens { get; set; } = new List<DfeItemSincronizado>();
    }
}