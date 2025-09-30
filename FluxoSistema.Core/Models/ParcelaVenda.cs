// Em Vendas.Core/ParcelaVenda.cs
using System;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa uma única parcela de um pagamento a prazo.
    /// </summary>
    public class ParcelaVenda
    {
        /// <summary>
        /// O número desta parcela (ex: 1, 2, 3...).
        /// </summary>
        public int NumeroParcela { get; set; }

        /// <summary>
        /// O valor monetário desta parcela específica.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// A data de vencimento desta parcela.
        /// </summary>
        public DateTime DataVencimento { get; set; }
    }
}