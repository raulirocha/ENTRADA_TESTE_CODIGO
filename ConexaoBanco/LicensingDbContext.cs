using FluxoSistema.Core;
using FluxoSistema.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace FluxoSistema.Infrastructure.ConexaoBanco
{
    public class LicensingDbContext : DbContext
    {
        public LicensingDbContext(DbContextOptions<LicensingDbContext> options) : base(options)
        {
        }

        public DbSet<LicencaCliente> LicencaClientes { get; set; }
        public DbSet<LicencaCnpj> LicencaCnpjs { get; set; }

        public DbSet<ConfiguracaoLicenca> ConfiguracaoLicencas { get; set; }
    }
}