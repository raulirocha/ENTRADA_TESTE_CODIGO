using System.Threading.Tasks;
using FluxoSistema.Application.Entrada.Repositories;
using FluxoSistema.Core.Models;
using FluxoSistema.Infrastructure.ConexaoBanco;
using Microsoft.EntityFrameworkCore;

namespace FluxoSistema.Infrastructure.Entrada.Repositories
{
    public class ProdutoFornecedorRepository : IProdutoFornecedorRepository
    {
        private readonly IDbContextFactory<MeuErpDbContext> _dbContextFactory;

        public ProdutoFornecedorRepository(IDbContextFactory<MeuErpDbContext> dbContextFactory)
        {
            _dbContextFactory = dbContextFactory;
        }

        public async Task<ProdutoFornecedor> FindVinculoAsync(int idFornecedor, string codigoFornecedor)
        {
            // Usamos a 'factory' para criar uma instância do contexto de banco de dados.
            // O 'using' garante que a conexão seja fechada corretamente após o uso.
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            // Usamos o Entity Framework Core para consultar a tabela de forma assíncrona.
            return await dbContext.Set<ProdutoFornecedor>()
                .FirstOrDefaultAsync(pf => pf.IdFornecedor == idFornecedor && pf.CodigoFornecedor == codigoFornecedor);
        }



        public async Task AddAsync(ProdutoFornecedor vinculo)
        {
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            dbContext.ProdutosFornecedores.Add(vinculo);
            await dbContext.SaveChangesAsync();
        }
    }
}