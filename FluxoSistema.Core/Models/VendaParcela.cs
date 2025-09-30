using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("venda_parcelas")]
    public class VendaParcela
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        // --- INÍCIO DA CORREÇÃO ---
        // Usamos o atributo [ForeignKey] para dizer ao EF Core que
        // esta propriedade 'PagamentoId' é a chave estrangeira para
        // a propriedade de navegação 'VendaPagamento' abaixo.
        [ForeignKey(nameof(VendaPagamento))]
        [Column("idpagamento")]
        public int PagamentoId { get; set; }

        // Adicionamos a propriedade de navegação para o "pai"
        public virtual VendaPagamento VendaPagamento { get; set; }
        // --- FIM DA CORREÇÃO ---

        [Column("numeroparcela")]
        public int NumeroParcela { get; set; }

        [Column("totalparcelas")]
        public int TotalParcelas { get; set; }

        [Column("valorparcela")]
        public decimal ValorParcela { get; set; }

        [Column("datavencimento")]
        public DateTime DataVencimento { get; set; }

        [Column("status")]
        public string Status { get; set; }

        [Column("observacao")]
        public string Observacao { get; set; }

        [Column("idvenda")]
        public int? VendaId { get; set; }

        [Column("id_nota_fiscal")]
        public int? IdNotaFiscal { get; set; }
    }
}