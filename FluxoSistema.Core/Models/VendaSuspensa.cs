// Em Vendas.Core/VendaSuspensa.cs
using System;

namespace FluxoSistema.Core.Models
{
    /// <summary>
    /// Representa os dados essenciais de uma venda suspensa para exibição em listas.
    /// </summary>
    public class VendaSuspensa
    {
        public int Id { get; set; }
        public DateTime DataAbertura { get; set; }
        public string NomeUsuario { get; set; } = string.Empty;
        public decimal ValorTotal { get; set; }
    }
}