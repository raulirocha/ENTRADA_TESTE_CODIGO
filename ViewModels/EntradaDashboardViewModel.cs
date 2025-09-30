// Em: FluxoSistema.Entrada/ViewModels/EntradaDashboardViewModel.cs

using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluxoSistema.Application.Entrada.Repositories;
using FluxoSistema.Application.Fiscal.NFe.Repositories;
using FluxoSistema.Application.Venda.Repositories;
using FluxoSistema.ComponentesComuns.ViewModels;
using FluxoSistema.Core.Models;
using Prism.Regions; // Importante para navegação

namespace FluxoSistema.Entrada.ViewModels
{
    public partial class EntradaDashboardViewModel : ObservableObject, INavigationAware
    {
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IRegionManager _regionManager;
        private readonly IPerfilFiscalRepository _perfilFiscalRepository;
        private readonly IProdutoFornecedorRepository _produtoFornecedorRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IClienteRepository _clienteRepository;
        private readonly IEntradaNotaFiscalRepository _entradaRepository;



        [ObservableProperty]
        private string _title = "Entradas Mercadoria";

        // Propriedade para controlar a visibilidade do nosso overlay
        [ObservableProperty]
        private bool _isNovaEntradaAberta;

        // Propriedade que servirá de DataContext para a nossa NovaEntradaView
        [ObservableProperty]
        private NovaEntradaViewModel _novaEntradaViewModel;


        private DfeEntradaSincronizada _notaAtual;
        private FornecedorParaCadastroDto _fornecedorParaCadastro;


        [ObservableProperty]
        private bool _isConfirmacaoCadastroVisivel;

        [ObservableProperty]
        private bool _isCadastroFornecedorVisivel;

        [ObservableProperty]
        private CadastroFornecedorViewModel _cadastroFornecedorViewModel;

        // 1. Adicione esta propriedade para guardar a lista de entradas
        [ObservableProperty]
        private ObservableCollection<EntradaNotaFiscal> _entradas = new();

        // 2. Adicione esta propriedade para saber qual item está selecionado (opcional, mas útil)
        [ObservableProperty]
        private EntradaNotaFiscal? _entradaSelecionada;



        public EntradaDashboardViewModel(IEmpresaRepository empresaRepository,
            IRegionManager regionManager,
            IPerfilFiscalRepository perfilFiscalRepository,
            IProdutoFornecedorRepository produtoFornecedorRepository,
            IProdutoRepository produtoRepository,
            IClienteRepository clienteRepository,
            IEntradaNotaFiscalRepository entradaRepository)
        {
            _empresaRepository = empresaRepository;
            _regionManager = regionManager;
            _perfilFiscalRepository = perfilFiscalRepository;
            _produtoFornecedorRepository = produtoFornecedorRepository;
            _produtoRepository = produtoRepository;
            _clienteRepository = clienteRepository;
            _entradaRepository = entradaRepository;
        }

        [RelayCommand]
        private void AbrirNovaEntrada()
        {
            // 1. Cria uma nova instância do ViewModel do formulário
            NovaEntradaViewModel = new NovaEntradaViewModel(
            _empresaRepository,
            _perfilFiscalRepository,
            _produtoFornecedorRepository,
            _produtoRepository,
            _clienteRepository,
            _entradaRepository);



            // 2. "Escuta" o evento que pede para fechar o formulário
            NovaEntradaViewModel.FecharRequisitado += OnFecharRequisitado;

            // 3. Mostra o overlay
            IsNovaEntradaAberta = true;
        }

        // Este método é chamado quando o formulário (NovaEntradaViewModel) pede para ser fechado
        private async void OnFecharRequisitado(bool sucesso)
        {
            // Esconde o overlay
            IsNovaEntradaAberta = false;

            // Limpa a referência antiga do ViewModel do formulário
            if (NovaEntradaViewModel != null)
            {
                NovaEntradaViewModel.FecharRequisitado -= OnFecharRequisitado;
                NovaEntradaViewModel = null;
            }

            if (sucesso)
            {
                // >>> LINHA ADICIONADA: Se a entrada foi salva, recarrega a lista para mostrar o novo item!
                await CarregarEntradasAsync();
            }
        }


        [RelayCommand]
        private void FecharAba(UserControl view)
        {
            // Verifica se a view a ser fechada não é nula
            if (view != null)
            {
                // CONDIÇÃO PRINCIPAL:
                // Se uma nova entrada estiver aberta (o overlay estiver visível)...
                if (IsNovaEntradaAberta)
                {
                    // ...mostra uma caixa de diálogo de confirmação.
                    var resultado = MessageBox.Show(
                        "Deseja realmente fechar? Todos os dados não salvos da entrada atual serão perdidos.",
                        "Confirmar Fechamento",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Warning);

                    // Se o usuário clicar em "Não", a gente para a execução do método aqui.
                    if (resultado == MessageBoxResult.No)
                    {
                        return; // Cancela a operação de fechar a aba.
                    }
                }

                // Se não havia uma entrada em andamento, OU se o usuário confirmou que quer fechar,
                // o código continua e remove a aba.
                _regionManager.Regions["ContentRegion"].Remove(view);
            }
        }


        // --- MÉTODOS DO PRISM PARA NAVEGAÇÃO ---

        public bool IsNavigationTarget(NavigationContext navigationContext) => true;

        public void OnNavigatedFrom(NavigationContext navigationContext) { }

      
        public async void OnNavigatedTo(NavigationContext navigationContext)
        {
            // >>> LINHA ADICIONADA: Carrega o DataGrid sempre que a tela for exibida.
            await CarregarEntradasAsync();


            // 1. Tenta extrair os dois parâmetros
            var notaRecebida = navigationContext.Parameters.GetValue<DfeEntradaSincronizada>("nota");
            var dadosFornecedor = navigationContext.Parameters.GetValue<FornecedorParaCadastroDto>("dadosFornecedor");

            if (notaRecebida != null && dadosFornecedor != null)
            {
                _notaAtual = notaRecebida;
                _fornecedorParaCadastro = dadosFornecedor; // Guarda os dados do fornecedor

                var fornecedor = await _clienteRepository.FindFornecedorByCnpjAsync(_notaAtual.CnpjEmitente);

                if (fornecedor == null)
                {
                    IsConfirmacaoCadastroVisivel = true;
                }
                else
                {
                    IniciarLancamentoEntrada(_notaAtual);
                }
            }
        }


        [RelayCommand]
        private void ConfirmarCadastroFornecedor()
        {
            IsConfirmacaoCadastroVisivel = false;

            // 2. Passa o DTO (pacote de dados) para o ViewModel de cadastro
            CadastroFornecedorViewModel = new CadastroFornecedorViewModel(_clienteRepository, _fornecedorParaCadastro);

            CadastroFornecedorViewModel.FecharRequisitado += OnCadastroFornecedorFechado;
            IsCadastroFornecedorVisivel = true;
        }

        [RelayCommand]
        private void CancelarCadastroFornecedor()
        {
            IsConfirmacaoCadastroVisivel = false;
            // Opcional: fechar a aba se o usuário não quiser cadastrar
            // Pode ser implementado no comando FecharAba
        }

        // --- MÉTODO CHAMADO QUANDO A TELA DE CADASTRO É FECHADA ---
        private void OnCadastroFornecedorFechado(Cliente novoFornecedor)
        {
            IsCadastroFornecedorVisivel = false; // Esconde o overlay de cadastro

            // Desvincula o evento para evitar memory leaks
            if (CadastroFornecedorViewModel != null)
            {
                CadastroFornecedorViewModel.FecharRequisitado -= OnCadastroFornecedorFechado;
                CadastroFornecedorViewModel = null;
            }

            // Se o cadastro foi um sucesso (não é nulo), continua para a tela de entrada
            if (novoFornecedor != null)
            {
                IniciarLancamentoEntrada(_notaAtual);
            }
        }

        // --- MÉTODO REUTILIZÁVEL PARA INICIAR A TELA DE ENTRADA ---
        private void IniciarLancamentoEntrada(DfeEntradaSincronizada nota)
        {
            // Cria o ViewModel do formulário
            NovaEntradaViewModel = new NovaEntradaViewModel(
             _empresaRepository,
             _perfilFiscalRepository,
             _produtoFornecedorRepository,
             _produtoRepository,
             _clienteRepository,
             _entradaRepository);
            NovaEntradaViewModel.FecharRequisitado += OnFecharRequisitado;

            // Manda o formulário carregar os dados da nota
            NovaEntradaViewModel.CarregarNota(nota);

            // Mostra o overlay da entrada
            IsNovaEntradaAberta = true;
        }


        private async Task CarregarEntradasAsync()
        {
            // Exemplo de como carregar os dados do repositório
            // Você pode chamar este método no construtor ou no OnNavigatedTo
            var listaDoBanco = await _entradaRepository.ListarTodasAsync(); // Supondo que exista este método

            Entradas.Clear();
            foreach (var entrada in listaDoBanco)
            {
                Entradas.Add(entrada);
            }
        }



    }
}