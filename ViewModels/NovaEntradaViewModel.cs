// Em: FluxoSistema.Entrada/ViewModels/NovaEntradaViewModel.cs

using System;
using System.Collections.ObjectModel;
using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluxoSistema.Application.Entrada.Repositories;
using FluxoSistema.Application.Fiscal.NFe.Repositories;
using FluxoSistema.Application.Venda.Repositories;
using FluxoSistema.Application.Venda.Security;
using FluxoSistema.Core.Models;
using FluxoSistema.ComponentesComuns.ViewModels;

namespace FluxoSistema.Entrada.ViewModels
{
    public partial class NovaEntradaViewModel : ObservableObject
    {

        private readonly IEmpresaRepository _empresaRepository;
        private readonly IPerfilFiscalRepository _perfilFiscalRepository;
        private readonly IProdutoFornecedorRepository _produtoFornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEntradaNotaFiscalRepository _entradaRepository;




        // Este evento será usado para avisar a tela principal (o Dashboard) que este formulário quer ser fechado.
        // O "bool" vai indicar se a operação foi um sucesso (true) ou um cancelamento (false).
        public event Action<bool> FecharRequisitado;


        [ObservableProperty]
        private string _title = "Nova Entrada";

        // Esta propriedade vai guardar os dados da nota que veio lá do Monitor DF-e.
        [ObservableProperty]
        private DfeEntradaSincronizada _notaFiscal;

        // Propriedade para a lista de empresas que vai popular o ComboBox
        [ObservableProperty]
        private ObservableCollection<Empresa> _empresas;

        // Propriedade para guardar a empresa que o usuário selecionou no ComboBox
        [ObservableProperty]
        private Empresa _empresaSelecionada;

        // Propriedade para a lista de perfis que vai popular o ComboBox
        [ObservableProperty]
        private ObservableCollection<PerfilFiscal> _perfisFiscais;

        // Propriedade para guardar o perfil que o usuário selecionou
        [ObservableProperty]
        private PerfilFiscal _perfilFiscalSelecionado;

        // Propriedade que a lista de produtos (ItemsControl) no XAML irá usar
        [ObservableProperty]
        private ObservableCollection<ItemEntradaViewModel> _itensDaNota;

        [ObservableProperty]
        private bool _isBuscandoProduto; // Controla a visibilidade do overlay de busca

        [ObservableProperty]
        private BuscaProdutoViewModel _buscaProdutoViewModel; // O DataContext para a BuscaProdutoView

        [ObservableProperty]
        private bool _isCadastrandoProduto; // Controla a visibilidade do overlay de cadastro

        [ObservableProperty]
        private CadastroProdutoViewModel _cadastroProdutoViewModel;

        [ObservableProperty]
        [NotifyCanExecuteChangedFor(nameof(SalvarCommand))]
        private bool _isSaving;




        public NovaEntradaViewModel(IEmpresaRepository empresaRepository,
            IPerfilFiscalRepository perfilFiscalRepository,
            IProdutoFornecedorRepository produtoFornecedorRepository,
            IProdutoRepository produtoRepository,
            IClienteRepository clienteRepository,
            IEntradaNotaFiscalRepository entradaRepository)
        {
            _empresaRepository = empresaRepository;
            _perfilFiscalRepository = perfilFiscalRepository;
            _produtoFornecedorRepository = produtoFornecedorRepository;
            _produtoRepository = produtoRepository;

            ItensDaNota = new ObservableCollection<ItemEntradaViewModel>();
            NotaFiscal = new DfeEntradaSincronizada();
            Empresas = new ObservableCollection<Empresa>();
            PerfisFiscais = new ObservableCollection<PerfilFiscal>();

            // Chama o método para carregar os dados assim que a tela for criada
            CarregarDependenciasAsync();
            _clienteRepository = clienteRepository;
            _entradaRepository = entradaRepository;
        }

        // Método assíncrono para carregar os dados do banco
        private async void CarregarDependenciasAsync()
        {
            try
            {
                var listaEmpresas = await _empresaRepository.GetAllAsync();
                Empresas.Clear();
                foreach (var empresa in listaEmpresas)
                {
                    Empresas.Add(empresa);
                }

                // Tenta pré-selecionar a empresa do usuário logado
                // Se a nota fiscal já tiver uma empresa, ela será sobrescrita depois no método CarregarNota
                EmpresaSelecionada = Empresas.FirstOrDefault(e => e.Id == SessaoUsuario.EmpresaId)
                                     ?? Empresas.FirstOrDefault(); // Se não encontrar, seleciona a primeira

                var todosOsPerfis = await _perfilFiscalRepository.GetAllAsync();
                PerfisFiscais.Clear();

                // Filtra a lista para pegar apenas os perfis onde o TipoOperacao é 'E' (Entrada)

                var perfisDeEntrada = todosOsPerfis.Where(p => p.TipoOperacao == "E");

                // Adiciona apenas os perfis filtrados ao ComboBox
                foreach (var perfil in perfisDeEntrada)
                {
                    PerfisFiscais.Add(perfil);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar a lista de empresas: {ex.Message}");
            }
        }

        // <summary>
        /// Este método é a chave. Ele é chamado pelo Dashboard quando a nota chega.
        /// </summary>
        // Em NovaEntradaViewModel.cs

        public void CarregarNota(DfeEntradaSincronizada nota)
        {
            NotaFiscal = nota;
            Title = $"Entrada NFe: {nota.NumeroNota} (Fornecedor: {nota.NomeEmitente})";

            if (Empresas.Any())
            {
                EmpresaSelecionada = Empresas.FirstOrDefault(e => e.Id == nota.IdEmpresa);
            }

            ItensDaNota.Clear();

            if (nota.Itens != null)
            {
                foreach (var item in nota.Itens)
                {
                    ItensDaNota.Add(new ItemEntradaViewModel(item));
                }

                // 3. ADICIONE A CHAMADA PARA A NOVA LÓGICA DE VINCULAÇÃO
                TentarVinculacaoAutomaticaAsync();
            }
        }

        
        /// <summary>
        /// Tenta vincular automaticamente os itens da nota fiscal com produtos existentes no sistema.
        /// Este método é acionado após o carregamento da nota. Ele segue os seguintes passos:
        /// 1. Busca o fornecedor na base de dados (tabela de clientes) usando o CNPJ da nota.
        /// 2. Se o fornecedor for encontrado, percorre cada item da nota.
        /// 3. Para cada item, consulta a tabela 'produto_fornecedor' para ver se já existe um vínculo salvo.
        /// 4. Se um vínculo for encontrado, busca o produto correspondente no sistema.
        /// 5. Se o produto for encontrado, atribui-o à propriedade 'ProdutoVinculado' do item,
        ///    o que atualiza automaticamente a interface do utilizador.
        /// </summary>
        private async void TentarVinculacaoAutomaticaAsync()
        {
            try
            {
                // Passo 1: Validação inicial dos dados necessários.
                if (NotaFiscal == null || string.IsNullOrWhiteSpace(NotaFiscal.CnpjEmitente) || !ItensDaNota.Any())
                {
                    return; // Sai se não tiver um CNPJ ou itens para processar.
                }

                // Passo 2: Buscar o fornecedor na base de dados usando o CNPJ da nota.
                var fornecedor = await _clienteRepository.FindFornecedorByCnpjAsync(NotaFiscal.CnpjEmitente);

                // Se não encontrarmos o fornecedor (ou ele não estiver cadastrado com tipo 'for'),
                // não podemos prosseguir com a vinculação automática.
                if (fornecedor == null)
                {
                    // Neste ponto, você poderia futuramente mostrar um aviso ao utilizador,
                    // mas por agora, apenas paramos o processo.
                    return;
                }

                // Obtemos o ID interno real do fornecedor.
                int idFornecedorDaNota = fornecedor.Id;

                // Passo 3: Percorrer cada item da nota para tentar a vinculação.
                foreach (var itemViewModel in ItensDaNota)
                {
                    // Ignora itens que não tenham um código de fornecedor.
                    if (string.IsNullOrWhiteSpace(itemViewModel.ItemOriginal.CodigoFornecedor))
                    {
                        continue;
                    }

                    // Passo 4: Tenta encontrar um vínculo na tabela 'produto_fornecedor'.
                    var vinculo = await _produtoFornecedorRepository.FindVinculoAsync(idFornecedorDaNota, itemViewModel.ItemOriginal.CodigoFornecedor);

                    // Se um vínculo foi encontrado...
                    if (vinculo != null)
                    {
                        // Passo 5: ...busca os dados completos do nosso produto interno.
                        var produtoDoMeuSistema = await _produtoRepository.GetByIdAsync(vinculo.IdProduto);

                        if (produtoDoMeuSistema != null)
                        {
                            // SUCESSO! Atualiza a propriedade no ViewModel do item.
                            // A interface irá reagir e exibir os dados do produto vinculado!
                            itemViewModel.ProdutoVinculado = produtoDoMeuSistema;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Em caso de qualquer erro inesperado durante o processo, informa o utilizador.
                MessageBox.Show($"Ocorreu um erro ao tentar vincular os produtos automaticamente: {ex.Message}",
                                "Erro na Vinculação Automática",
                                MessageBoxButton.OK,
                                MessageBoxImage.Error);
            }
        }



        // Comando para o botão "Vincular" que está em cada item da lista
        [RelayCommand]
        private void VincularProduto(ItemEntradaViewModel itemParaVincular)
        {
            // Validação para garantir que temos um item para trabalhar
            if (itemParaVincular == null) return;

            // Dica de ouro: Usamos a descrição do produto da nota como termo de busca inicial
            string termoBusca = itemParaVincular.ItemOriginal.DescricaoProduto;

            // Criamos o ViewModel da busca. A parte mais importante é o 'callback'.
            // O callback é a ação que será executada QUANDO o utilizador selecionar um produto ou cancelar.
            BuscaProdutoViewModel = new BuscaProdutoViewModel(
                _produtoRepository,
                termoBusca,
                produtoSelecionado => {

                    // Este código só executa DEPOIS que a busca é fechada.
                    if (produtoSelecionado != null)
                    {
                        // Se o utilizador selecionou um produto, atualizamos a UI...
                        itemParaVincular.ProdutoVinculado = produtoSelecionado;

                        // ...E ensinamos o sistema para o futuro, salvando o vínculo.
                        SalvarNovoVinculoAsync(itemParaVincular, produtoSelecionado);
                    }

                    // Esconde o overlay de busca, voltando para a tela de entrada.
                    IsBuscandoProduto = false;
                    BuscaProdutoViewModel = null; // Limpa a referência
                });

            // Mostra o overlay de busca de produto.
            IsBuscandoProduto = true;
        }

        // --- MÉTODO AUXILIAR PARA SALVAR O VÍNCULO ---
        private async void SalvarNovoVinculoAsync(ItemEntradaViewModel itemVinculado, Produto produtoVinculado)
        {
            try
            {
                // 1. Precisamos do ID do Fornecedor. Buscamos novamente.
                var fornecedor = await _clienteRepository.FindFornecedorByCnpjAsync(NotaFiscal.CnpjEmitente);
                if (fornecedor == null) return;

                var vinculoExistente = await _produtoFornecedorRepository.FindVinculoAsync(fornecedor.Id, itemVinculado.ItemOriginal.CodigoFornecedor);

                // Se o vínculo já existe na base de dados, não precisamos fazer nada.
                if (vinculoExistente != null)
                {
                    return; // Apenas saímos do método.
                }



                // 2. Criamos o novo objeto de vínculo
                var novoVinculo = new ProdutoFornecedor
                {
                    IdFornecedor = fornecedor.Id,
                    IdProduto = produtoVinculado.Id,
                    CodigoFornecedor = itemVinculado.ItemOriginal.CodigoFornecedor,
                    DescricaoFornecedor = itemVinculado.ItemOriginal.DescricaoProduto,
                    UnidadeFornecedor = itemVinculado.ItemOriginal.Unidade,
                    // Inicialmente, fator de conversão é 1. Poderá ser ajustado numa tela de gestão.
                    FatorConversaoEntrada = 1
                };

                // 3. Adicionamos o novo método ao repositório para salvar (próximo passo)
                await _produtoFornecedorRepository.AddAsync(novoVinculo);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao salvar o vínculo do produto: {ex.Message}");
            }
        }

        [RelayCommand]
        private void CadastrarProduto(ItemEntradaViewModel itemParaCadastrar)
        {
            // Validação para garantir que temos um item para trabalhar
            if (itemParaCadastrar == null) return;

            // Criamos o ViewModel do cadastro. Passamos o repositório, os dados do item da nota
            // e o importantíssimo 'callback'.
            CadastroProdutoViewModel = new CadastroProdutoViewModel(
                _produtoRepository,
                itemParaCadastrar.ItemOriginal,
                produtoCadastrado => {

                    // Este código só executa DEPOIS que a tela de cadastro é fechada.
                    if (produtoCadastrado != null)
                    {
                        // Se um novo produto foi salvo, atualizamos a UI...
                        itemParaCadastrar.ProdutoVinculado = produtoCadastrado;

                        // ...E imediatamente salvamos o vínculo para automatizar no futuro.
                        SalvarNovoVinculoAsync(itemParaCadastrar, produtoCadastrado);
                    }

                    // Esconde o overlay de cadastro, voltando para a tela de entrada.
                    IsCadastrandoProduto = false;
                    CadastroProdutoViewModel = null; // Limpa a referência
                });

            // Mostra o overlay de cadastro de produto.
            IsCadastrandoProduto = true;
        }

        private bool CanSalvar()
        {
            return !_isSaving;
        }

        [RelayCommand(CanExecute = nameof(CanSalvar))]
        private async Task Salvar()
        {
            if (_isSaving) return;

            try
            {
                IsSaving = true;

                // --- ETAPA 1: Validação ---
                if (EmpresaSelecionada == null)
                {
                    MessageBox.Show("Selecione a empresa de destino.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (PerfilFiscalSelecionado == null)
                {
                    MessageBox.Show("Selecione a operação (perfil fiscal).", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (ItensDaNota.Any(item => item.ProdutoVinculado == null))
                {
                    MessageBox.Show("Todos os produtos da nota devem ser vinculados a um produto do sistema antes de salvar.", "Validação", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                var fornecedor = await _clienteRepository.FindFornecedorByCnpjAsync(NotaFiscal.CnpjEmitente);
                if (fornecedor == null)
                {
                    MessageBox.Show("O fornecedor não foi encontrado ou não está cadastrado. A operação não pode continuar.", "Erro Crítico", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                var acoesPermitidas = await _perfilFiscalRepository.BuscarAcoesDoPerfilAsync(PerfilFiscalSelecionado.Id);
                bool deveAfetarEstoqueFisico = acoesPermitidas.Contains("AFETAESTOQUEFISICO");
                bool deveAfetarEstoqueFiscal = acoesPermitidas.Contains("AFETAESTOQUEFISCAL");

                // --- ETAPA 2: Montagem dos Objetos ---

                // Monta o Cabeçalho da Nota
                var cabecalhoNota = new EntradaNotaFiscal
                {
                    ChaveAcesso = NotaFiscal.ChaveAcesso,
                    NumeroNota = NotaFiscal.NumeroNota,
                    Serie = NotaFiscal.Serie,
                    DataEmissao = NotaFiscal.DataEmissao ?? DateTime.MinValue,
                    DataEntrada = DateTime.UtcNow, // Ou um campo de data da tela
                    IdFornecedor = fornecedor.Id,
                    ValorTotal = NotaFiscal.ValorTotal ?? 0m,
                    ValorProdutos = NotaFiscal.Itens.Sum(i => i.ValorTotal ?? 0m),
                                                                            
                    XmlCompleto = NotaFiscal.XmlCompleto,
                    BaseIcms = NotaFiscal.ValorIcms ?? 0m,
                    IdEmpresa = EmpresaSelecionada.Id,
                    IdPerfilFiscal = PerfilFiscalSelecionado.Id,
                    Status = "FINALIZADA"
                };

                // Monta a Lista de Itens (Movimentos de Estoque)
                var listaMovimentos = new List<MovimentoEstoque>();
                foreach (var itemViewModel in ItensDaNota)
                {
                    var movimento = new MovimentoEstoque
                    {
                        ProdutoId = itemViewModel.ProdutoVinculado.Id,
                        ClienteId = fornecedor.Id, // Vincula o movimento ao fornecedor
                        UsuarioId = SessaoUsuario.UsuarioLogadoID, // Pega o ID do usuário logado
                        NomeProduto = itemViewModel.ProdutoVinculado.Descricao_PROD,
                        OrigemMovimento = "ENTRADA POR NF-e",
                        TipoMovimento = "E",
                        StatusMovimento = "ATIVO",
                        Quantidade = itemViewModel.ItemOriginal.Quantidade ?? 0m,
                        ValorUnitario = itemViewModel.ItemOriginal.ValorUnitario ?? 0m,
                        ValorTotalBruto = itemViewModel.ItemOriginal.ValorTotal ?? 0m,
                        ValorTotalLiquido = itemViewModel.ItemOriginal.ValorTotal ?? 0m, // Ajustar se houver descontos no item
                        Cfop = itemViewModel.ItemOriginal.Cfop,
                        Ncm = itemViewModel.ItemOriginal.Ncm,
                        Unidade = itemViewModel.ItemOriginal.Unidade,
                        IdEmpresa = EmpresaSelecionada.Id,
                        PerfilFiscalId = PerfilFiscalSelecionado.Id,
                        AfetaEstoqueFisico = deveAfetarEstoqueFisico,
                        AfetaEstoqueFiscal = deveAfetarEstoqueFiscal
                        // Mapeie aqui os outros campos fiscais do item a partir de itemViewModel.ItemOriginal
                    };
                    listaMovimentos.Add(movimento);
                }

                // --- ETAPA 3: Chamada ao Repositório ---
                await _entradaRepository.RegistrarEntradaAsync(cabecalhoNota, listaMovimentos);

                MessageBox.Show("Nota de entrada registrada com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
                FecharRequisitado?.Invoke(true); // Fecha o formulário com sucesso
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro grave ao salvar a nota de entrada:\n{ex.Message}", "Erro", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                IsSaving = false;
            }
        }

        // O método Cancelar não precisa de mudanças
        [RelayCommand]
        private void Cancelar()
        {
            FecharRequisitado?.Invoke(false);
        }
    }
}