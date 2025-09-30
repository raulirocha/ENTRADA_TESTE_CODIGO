
using FluxoSistema.Application.Dfe.Repositories;
using FluxoSistema.Core.Models;
using FluxoSistema.Infrastructure.ConexaoBanco;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace FluxoSistema.Infrastructure.Dfe.Repositories
{
    public class ControleDfeRepository : IControleDfeRepository
    {
        private readonly MeuErpDbContext _context;

        public ControleDfeRepository(MeuErpDbContext context)
        {
            _context = context;
        }

        public async Task<ControleDfe> GetControlePorCnpjAsync(string cnpj)
        {
            return await _context.ControlesDfe.FirstOrDefaultAsync(c => c.CnpjDestinatario == cnpj);
        }

        public async Task SalvarControleAsync(ControleDfe controle)
        {
            var existente = await _context.ControlesDfe.FindAsync(controle.Id);
            if (existente == null)
            {
                _context.ControlesDfe.Add(controle);
            }
            else
            {
                _context.Entry(existente).CurrentValues.SetValues(controle);
            }
            await _context.SaveChangesAsync();
        }
    }
}