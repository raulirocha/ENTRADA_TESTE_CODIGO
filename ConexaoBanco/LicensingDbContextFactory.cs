// Em: FluxoSistema.Infrastructure/ConexaoBanco/LicensingDbContextFactory.cs
// VERSÃO FINAL COM SUA STRING DE CONEXÃO

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FluxoSistema.Infrastructure.ConexaoBanco
{
    public class LicensingDbContextFactory : IDesignTimeDbContextFactory<LicensingDbContext>
    {
        public LicensingDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<LicensingDbContext>();

            // Endereço do banco de dados inserido diretamente para a ferramenta de linha de comando.
            var connectionString = "Host=35.198.50.217;Database=fluxo;Username=postgres;Password=FluxoDb2025@";

            optionsBuilder.UseNpgsql(connectionString,
                o => o.MigrationsAssembly("FluxoSistema.Licensing.Admin"));

            return new LicensingDbContext(optionsBuilder.Options);
        }
    }
}