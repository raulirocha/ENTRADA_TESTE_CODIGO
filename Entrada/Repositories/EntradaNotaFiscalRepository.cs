using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluxoSistema.Application.Entrada.Repositories;
using FluxoSistema.Core.Models;
using FluxoSistema.Infrastructure.ConexaoBanco;
// 1. ADICIONE O USING PARA O SERVIÇO DE ESTOQUE
using FluxoSistema.Infrastructure.Estoque.ServicosEmMemoria;
using Microsoft.EntityFrameworkCore;

namespace FluxoSistema.Infrastructure.Entrada.Repositories
{
    public class EntradaNotaFiscalRepository : IEntradaNotaFiscalRepository
    {
        private readonly IDbContextFactory<MeuErpDbContext> _dbContextFactory;
        // 2. DECLARE O SERVIÇO DE ESTOQUE COMO UMA DEPENDÊNCIA
        private readonly ServicoEstoqueEmMemoria _servicoEstoque;

        // 3. ATUALIZE O CONSTRUTOR PARA RECEBER O SERVIÇO
        public EntradaNotaFiscalRepository(IDbContextFactory<MeuErpDbContext> dbContextFactory, ServicoEstoqueEmMemoria servicoEstoque)
        {
            _dbContextFactory = dbContextFactory;
            _servicoEstoque = servicoEstoque; // Armazene a instância
        }

        // 4. REESCREVA COMPLETAMENTE O MÉTODO PARA USAR O MOTOR DE ESTOQUE
        public async Task<int> RegistrarEntradaAsync(EntradaNotaFiscal notaFiscal, List<MovimentoEstoque> movimentos)
        {
            // A lógica agora segue um padrão mais robusto:
            // 1. Salva o cabeçalho da nota primeiro para obter um ID.
            // 2. Envia a tarefa de atualização de estoque para o serviço em fila.
            // 3. Se o serviço falhar, desfaz o salvamento do cabeçalho.
            // 4. Se o serviço tiver sucesso, confirma a operação.

            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();
            await using var transaction = await dbContext.Database.BeginTransactionAsync();

            try
            {
                // --- ETAPA 1: Salvar o Cabeçalho da Nota (para obter o ID) ---
                notaFiscal.DataCadastro = DateTime.UtcNow;
                await dbContext.EntradasNotasFiscais.AddAsync(notaFiscal);
                // Salva as alterações para que o EF preencha a propriedade notaFiscal.Id
                await dbContext.SaveChangesAsync();

                // --- ETAPA 2: Preparar e Enviar os Movimentos para o Motor de Estoque ---

                // Prepara os argumentos para o serviço.
                var movimentacaoArgs = new MovimentacaoArgs
                {
                    // Para uma entrada, não precisamos validar se há saldo negativo.
                    ValidarSaldo = false
                };

                foreach (var movimento in movimentos)
                {
                    // É crucial vincular cada movimento ao ID do cabeçalho que acabamos de salvar.
                    movimento.DocumentoId = notaFiscal.Id;
                    movimentacaoArgs.Movimentos.Add(movimento);
                }

                // Envia a requisição para a fila de processamento de estoque.
                // O `await` aqui espera pela "promessa" de que o serviço irá processar e retornar um resultado.
                ResultadoEstoque resultadoEstoque = await _servicoEstoque.EnviarParaAtualizacaoAsync(movimentacaoArgs);

                // --- ETAPA 3: Verificar o Resultado da Operação de Estoque ---

                if (!resultadoEstoque.Sucesso)
                {
                    // Se o motor de estoque retornou um erro, lançamos uma exceção.
                    // Isso fará com que o bloco 'catch' abaixo reverta a transação,
                    // desfazendo o salvamento do cabeçalho da nota e mantendo a consistência dos dados.
                    throw new InvalidOperationException($"Falha ao processar o estoque: {resultadoEstoque.Mensagem}");
                }

                // --- ETAPA 4: Se tudo correu bem, confirma a transação ---
                await transaction.CommitAsync();

                return notaFiscal.Id; // Retorna o ID da nota salva.
            }
            catch (Exception)
            {
                // Se qualquer erro ocorreu, desfazemos tudo.
                await transaction.RollbackAsync();
                // Lança a exceção para que o ViewModel (a tela) saiba que algo deu errado.
                throw;
            }
        }


        public async Task<List<EntradaNotaFiscal>> ListarTodasAsync()
        {
            // 1. Pede um contexto de base de dados à fábrica.
            await using var dbContext = await _dbContextFactory.CreateDbContextAsync();

            // 2. Constrói a consulta para buscar as entradas.
            //    - AsNoTracking() é uma otimização para consultas de apenas leitura.
            //    - OrderByDescending() garante que as notas mais recentes apareçam primeiro.
            var entradas = await dbContext.EntradasNotasFiscais
                .AsNoTracking()
                .OrderByDescending(e => e.DataEmissao)
                .ToListAsync();

            // 3. Retorna a lista de entradas encontradas.
            return entradas;
        }


    }
}