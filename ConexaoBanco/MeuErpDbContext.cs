using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluxoSistema.Core.Models; 

namespace FluxoSistema.Infrastructure.ConexaoBanco
{
    /// <summary>
    /// Representa a sessão com a base de dados e atua como o nosso "Tradutor" principal.
    /// Contém a declaração de todas as tabelas que o Entity Framework irá gerir.
    /// </summary>
    public class MeuErpDbContext : DbContext
    {
        // Todas as suas tabelas declaradas, prontas para serem usadas.
        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Configuracao> Configuracoes { get; set; }
        public DbSet<ForPagamento> FormasDePagamento { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<Endereco> Enderecos { get; set; }
        public DbSet<Produto> Produtos { get; set; }
        public DbSet<Marca> Marcas { get; set; }
        public DbSet<Vendedor> Vendedores { get; set; }
        public DbSet<Permissao> Permissoes { get; set; }
        public DbSet<TipoPermissao> TiposPermissao { get; set; }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<PdvVenda> PdvVendas { get; set; }
        public DbSet<ItemVenda> PdvItensVenda { get; set; }
        public DbSet<VendaPagamento> VendaPagamentos { get; set; }
        public DbSet<VendaParcela> VendaParcelas { get; set; }
        public DbSet<FluxoSistema.Core.Models.Venda> Vendas { get; set; }
        public DbSet<MovimentoEstoque> MovimentosEstoque { get; set; }
        public DbSet<SistemaAcao> SistemaAcoes { get; set; }
        public DbSet<FiscalPerfilRegraAcao> FiscalPerfilRegrasAcao { get; set; }
        public DbSet<PerfilFiscal> FiscalPerfis { get; set; }

        public DbSet<Cfop> Cfops { get; set; }

        public DbSet<NumeracaoNfe> NumeracaoNfes { get; set; }
        public DbSet<NumeracaoNfce> NumeracaoNfces { get; set; }
        public DbSet<ClassificacaoFiscal> ClassificacoesFiscais { get; set; }
        public DbSet<RegraFiscal> RegrasFiscais { get; set; }

        public DbSet<NotaFiscalPagamento> NotaFiscalPagamentos { get; set; }
        public DbSet<NotaFiscalParcela> NotaFiscalParcelas { get; set; }

        public DbSet<IbptAliquota> IbptAliquotas { get; set; }
        public DbSet<IbptVersao> IbptVersoes { get; set; }

        public DbSet<ControleDfe> ControlesDfe { get; set; }
        public DbSet<DfeEntradaSincronizada> DfeEntradasSincronizadas { get; set; }
        public DbSet<DfeItemSincronizado> DfeItensSincronizados { get; set; }

        public DbSet<ProdutoFornecedor> ProdutosFornecedores { get; set; }
        public DbSet<NumeracaoProduto> NumeracaoProduto { get; set; }

        public DbSet<EntradaNotaFiscal> EntradasNotasFiscais { get; set; }

        public DbSet<ProdutoEstoqueSaldo> ProdutoEstoqueSaldos { get; set; }

        /// <summary>
        /// Este método é chamado pelo EF Core para configurar a conexão com o banco.
        /// </summary>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IniConfigManager.LoadConfig();
            // A lógica de conexão continua a mesma e está correta.
            string connectionString = IniConfigManager.ConnectionString;

            optionsBuilder.UseNpgsql(connectionString)
                .LogTo(message => Debug.WriteLine(message), LogLevel.Information);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Diz ao EF Core que a entidade FiscalPerfilRegraAcao...
            modelBuilder.Entity<FiscalPerfilRegraAcao>(entity =>
            {
                // ...tem uma chave primária composta pelas propriedades PerfilFiscalId e AcaoId.
                entity.HasKey(e => new { e.PerfilFiscalId, e.AcaoId });
            });

            // Configura a entidade NumeracaoProduto para não ter chave primária
            modelBuilder.Entity<NumeracaoProduto>(entity =>
            {
                entity.HasNoKey();
            });

            
            // Configurar o relacionamento 1 para 1 entre Produto e seu Saldo.
            // Isso explica ao EF que a chave primária de 'ProdutoEstoqueSaldo'
            // é também uma chave estrangeira para a tabela 'Produto'.
           
            modelBuilder.Entity<ProdutoEstoqueSaldo>(entity =>
            {
                entity.HasOne(saldo => saldo.Produto)
                      .WithOne() // Não há necessidade de uma propriedade de navegação inversa em Produto
                      .HasForeignKey<ProdutoEstoqueSaldo>(saldo => saldo.ProdutoId);
            });





            // Este loop percorre todas as entidades (tabelas) do seu DbContext.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                // Este loop percorre todas as propriedades (colunas) de cada entidade.
                foreach (var property in entityType.GetProperties())
                {
                    // --- Lógica separada para cada tipo de DateTime ---

                    // Se a propriedade for do tipo DateTime? (nulável)
                    if (property.ClrType == typeof(DateTime?))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime?>(property.Name)
                            .HasConversion(
                                // Ao SALVAR: Se tiver valor, converte para UTC. Se não, continua nulo.
                                v => v.HasValue ? v.Value.ToUniversalTime() : v,
                                // Ao LER: Se tiver valor, "carimba" como UTC. Se não, continua nulo.
                                v => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : v);
                    }
                    // Se a propriedade for do tipo DateTime (não-nulável)
                    else if (property.ClrType == typeof(DateTime))
                    {
                        modelBuilder.Entity(entityType.ClrType)
                            .Property<DateTime>(property.Name)
                            .HasConversion(
                                // Ao SALVAR: Converte o valor para UTC (nunca será nulo).
                                v => v.ToUniversalTime(),
                                // Ao LER: "Carimba" o valor lido como sendo UTC (nunca será nulo).
                                v => DateTime.SpecifyKind(v, DateTimeKind.Utc));
                    }
                }
            }
            
        }
    }
}