// Em Vendas.Core/VendaPagamento.cs
using System.Collections.Generic; // Adicionado para a lista de parcelas
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FluxoSistema.Core.Models
{
    // --- CORREÇÃO AQUI ---
    // Corrigimos o nome da tabela para "venda_pagamento"
    [Table("venda_pagamento")]
    public class VendaPagamento
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Column("idvenda")]
        public int? VendaId { get; set; }

        // --- CORREÇÃO AQUI ---
        // A sua coluna original era 'formapagamento' (string)
        [Column("formapagamento")]
        public string FormaPagamento { get; set; }

        [Column("valorpago")]
        public decimal ValorPago { get; set; }

        [Column("tiponf")]
        public string TipoNf { get; set; } // Usamos string para corresponder ao 'character varying'

        [Column("id_nota_fiscal")]
        public int? IdNotaFiscal { get; set; }


        // Propriedade de navegação: um pagamento pode ter várias parcelas
        public virtual ICollection<VendaParcela> Parcelas { get; set; } = new List<VendaParcela>();
    }
}