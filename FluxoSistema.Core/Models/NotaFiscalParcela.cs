using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("nota_fiscal_parcela")]
    public class NotaFiscalParcela
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_pagamento")]
        public int PagamentoId { get; set; }

        [Column("id_nota_fiscal")]
        public int IdNotaFiscal { get; set; }

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

        // Propriedade de navegação para o "pai" (o Pagamento)
        public virtual NotaFiscalPagamento Pagamento { get; set; }
    }
}