// Em Vendas.Core/AcaoFiscal.cs
namespace FluxoSistema.Core.Models
{
    public enum AcaoFiscal
    {
        // O valor 0 é reservado para 'indefinido' ou erro.
        Indefinida = 0,
        AFETAESTOQUEFISICO,
        AFETAESTOQUEFISCAL,
        ConsideraFaturamento,
        GeraComissao
        // Adicione outras ações aqui no futuro.
    }
}