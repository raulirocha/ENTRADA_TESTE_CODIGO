// Salve como: FluxoSistema.Core/Models/DfeItemSincronizado.cs
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("dfe_item_sincronizado")]
    public class DfeItemSincronizado
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idnotasincronizada")]
        public int IdNota_Sincronizada { get; set; }

        [Column("numeroitem")]
        public int? NumeroItem { get; set; }

        [Column("codigofornecedor")]
        public string? CodigoFornecedor { get; set; }

        [Column("descricaoproduto")]
        public string? DescricaoProduto { get; set; }

        [Column("codigobarras")]
        public string? CodigoBarras { get; set; }

        [Column("unidade")]
        public string? Unidade { get; set; }

        [Column("ncm")]
        public string? Ncm { get; set; }

        [Column("cfop")]
        public string? Cfop { get; set; }

        [Column("cst")]
        public string? Cst { get; set; }

        [Column("origemicms")]
        public string? Origem { get; set; }

        [Column("aliquotaicms")]
        public decimal? AliquotaIcms { get; set; }

        [Column("quantidade")]
        public decimal? Quantidade { get; set; }

        [Column("valorunitario")]
        public decimal? ValorUnitario { get; set; }

        [Column("valortotal")]
        public decimal? ValorTotal { get; set; }

        [Column("informacaoadicional")]
        public string? InformacaoAdicional { get; set; }

        [Column("ceantrib")]
        public string? CeanTrib { get; set; }

        [Column("utrib")]
        public string? UTrib { get; set; }

        [Column("qtrib")]
        public decimal? QTrib { get; set; }

        [Column("vuntrib")]
        public decimal? VUnTrib { get; set; }

        [Column("vdesc")]
        public decimal? VDesc { get; set; }

        [Column("voutro")]
        public decimal? VOutro { get; set; }

        [Column("vtottrib")]
        public decimal? VTotTrib { get; set; }

        [Column("ncmv")]
        public string? Ncmv { get; set; }

        [Column("cest")]
        public string? Cest { get; set; }

        [Column("cenq")]
        public string? Cenq { get; set; }

        // Propriedade de Navegação: Um item pertence a uma nota.
        [ForeignKey("IdNota_Sincronizada")]
        public virtual DfeEntradaSincronizada Nota { get; set; }
    }
}