using Microsoft.EntityFrameworkCore;
using System;

namespace FluxoSistema.Infrastructure.ConexaoBanco
{
    /// <summary>
    /// Uma implementação simples de IDbContextFactory compatível com o Prism
    /// e outros contentores de DI que não têm um método de registo nativo.
    /// </summary>
    public class PrismDbContextFactory<TContext> : IDbContextFactory<TContext> where TContext : DbContext
    {
        private readonly Func<TContext> _createDbContext;

        public PrismDbContextFactory(Func<TContext> createDbContext)
        {
            _createDbContext = createDbContext;
        }

        public TContext CreateDbContext()
        {
            // Simplesmente chama a função que sabe como criar um novo DbContext.
            return _createDbContext();
        }
    }
}