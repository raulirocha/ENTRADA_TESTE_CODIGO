using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    [Table("nota_fiscal_pagamento")]
    public class NotaFiscalPagamento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("id_nota_fiscal")]
        public int IdNotaFiscal { get; set; }

        [Column("formapagamento")]
        public string FormaPagamento { get; set; }

        [Column("valorpago")]
        public decimal ValorPago { get; set; }

        [Column("tiponf")]
        public string TipoNf { get; set; }

        // Propriedade de navegação: um pagamento pode ter várias parcelas
        public virtual ICollection<NotaFiscalParcela> Parcelas { get; set; } = new List<NotaFiscalParcela>();
    }
}