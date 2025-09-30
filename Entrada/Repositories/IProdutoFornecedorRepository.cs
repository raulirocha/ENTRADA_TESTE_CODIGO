using FluxoSistema.Core.Models;
using System.Threading.Tasks;

namespace FluxoSistema.Application.Entrada.Repositories
{
    public interface IProdutoFornecedorRepository
    {
        /// <summary>
        /// Procura por um vínculo de produto-fornecedor com base no ID do fornecedor e no código do produto utilizado por ele.
        /// </summary>
        /// <param name="idFornecedor">O ID do fornecedor (emitente da nota).</param>
        /// <param name="codigoFornecedor">O código (part number) que o fornecedor usa para o produto.</param>
        /// <returns>O objeto ProdutoFornecedor se o vínculo for encontrado; caso contrário, retorna null.</returns>
        Task<ProdutoFornecedor> FindVinculoAsync(int idFornecedor, string codigoFornecedor);


        Task AddAsync(ProdutoFornecedor vinculo);
    }
}