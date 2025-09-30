// Em Vendas.Core/NaturezaOperacao.cs
namespace FluxoSistema.Core.Models
{
    public enum NaturezaOperacao
    {
        // O valor 0 é reservado para 'indefinido' ou erro.
        Indefinida = 0,
        Venda,
        Orcamento,
        Devolucao
        // Adicione outras naturezas aqui no futuro.
    }
}