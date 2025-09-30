// Salve como: FluxoSistema.Infrastructure/Dfe/Repositories/DfeRepository.cs

using FluxoSistema.Application.Dfe.Repositories;
using FluxoSistema.Core.Models;
using FluxoSistema.Infrastructure.ConexaoBanco; // Verifique se este é o namespace correto do seu DbContext
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FluxoSistema.Infrastructure.Dfe.Repositories
{
    /// <summary>
    /// Implementação concreta do repositório de DF-e,
    /// responsável por acessar o banco de dados usando Entity Framework Core.
    /// </summary>
    public class DfeRepository : IDfeRepository
    {
        private readonly MeuErpDbContext _context;

        // O DbContext é injetado pelo sistema de injeção de dependência do Prism.
        public DfeRepository(MeuErpDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Busca as notas sincronizadas de uma empresa específica no banco de dados.
        /// </summary>
        public async Task<IEnumerable<DfeEntradaSincronizada>> GetNotasSincronizadasAsync(int idEmpresa)
        {
            // Usamos o Entity Framework para criar a consulta:
            // 1. Acessa a tabela 'DfeEntradasSincronizadas'.
            // 2. Filtra (Where) para pegar apenas as notas da empresa logada.
            // 3. Ordena (OrderByDescending) para que as notas mais novas apareçam primeiro.
            // 4. Executa a consulta no banco de forma assíncrona (ToListAsync).
            return await _context.DfeEntradasSincronizadas // <-- ATENÇÃO AQUI!
                                   .Include(n => n.Itens)
                                   .Where(d => d.IdEmpresa == idEmpresa)
                                   .OrderByDescending(d => d.DataEmissao)
                                   .ToListAsync();
        }

        public async Task<DfeEntradaSincronizada> GetNotaPorChaveAcessoAsync(string chaveAcesso)
        {
            return await _context.DfeEntradasSincronizadas
                                   .FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
        }

        public async Task SalvarNotaAsync(DfeEntradaSincronizada nota)
        {
            // Este método assume que a nota é sempre nova, pois já verificamos se existe antes
            await _context.DfeEntradasSincronizadas.AddAsync(nota);
            await _context.SaveChangesAsync();
        }


        public async Task AtualizarStatusManifestoAsync(string chaveAcesso, string novoStatus)
        {
            var nota = await _context.DfeEntradasSincronizadas.FirstOrDefaultAsync(n => n.ChaveAcesso == chaveAcesso);
            if (nota != null)
            {
                nota.ManifestoStatus = novoStatus;
                await _context.SaveChangesAsync();
            }
        }

        public async Task AtualizarNotaAsync(DfeEntradaSincronizada nota)
        {
            // O Entity Framework rastreia a entidade, então só precisamos salvar as mudanças.
            _context.DfeEntradasSincronizadas.Update(nota);
            await _context.SaveChangesAsync();
        }
    }
}