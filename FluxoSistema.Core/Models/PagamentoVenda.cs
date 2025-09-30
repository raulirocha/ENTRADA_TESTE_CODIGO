// Em Vendas.Core/PagamentoVenda.cs
using System.Collections.Generic;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa um pagamento efetuado em uma venda específica.
    /// Uma única venda pode ter múltiplos pagamentos.
    /// </summary>
    public class PagamentoVenda
    {
        /// <summary>
        /// O ID da forma de pagamento utilizada.
        /// </summary>
        public int FormaPagamentoId { get; set; }

        /// <summary>
        /// A descrição da forma de pagamento (para exibição fácil).
        /// </summary>
        public string FormaPagamentoDescricao { get; set; } = string.Empty;

        /// <summary>
        /// O valor total pago nesta modalidade.
        /// </summary>
        public decimal Valor { get; set; }

        /// <summary>
        /// A lista de parcelas, se houver.
        /// Se não for parcelado, esta lista estará vazia.
        /// </summary>
        public List<ParcelaVenda> Parcelas { get; set; } = new();

        // Esta propriedade irá transportar o código fiscal (ex: 1, 15)
        // que veio do cadastro da forma de pagamento.
        public string FormaPagamentoCodigoFiscal { get; set; }
    }
}