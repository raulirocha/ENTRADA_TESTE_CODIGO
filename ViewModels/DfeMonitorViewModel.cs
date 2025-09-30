// Salve em: FluxoSistema.Dfe/ViewModels/DfeMonitorViewModel.cs

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls; // <-- NECESSÁRIO PARA O UserControl NO COMANDO
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using DFe.Utils;
using FluxoSistema.Application.Dfe.Repositories;
using FluxoSistema.Application.Dfe.Services;
using FluxoSistema.Application.Fiscal.NFe.Repositories;
using FluxoSistema.Application.Geral;
using FluxoSistema.Application.Venda.Security;
using FluxoSistema.Core.Models;
using Prism.Regions;


namespace FluxoSistema.Dfe.ViewModels
{
    public partial class DfeMonitorViewModel : ObservableObject
    {
        private readonly IDfeRepository _dfeRepository;
        private readonly IEmpresaRepository _empresaRepository;
        private readonly IRegionManager _regionManager; // <-- NOVO: Para gerenciar as abas
        private readonly IDfeSefazService _dfeSefazService;
        private readonly ILoadingService _loadingService;

        // "Trava" para evitar a condição de corrida na inicialização
        private bool _isInitializing = false;

        [ObservableProperty]
        private string _title = "Monitor (DF-e)";

        [ObservableProperty]
        private ObservableCollection<DfeEntradaSincronizada> _notas;

        [ObservableProperty]
        private ObservableCollection<Empresa> _empresas;

        [ObservableProperty]
        private Empresa _empresaSelecionada;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(IsItemSelected))]
        private DfeEntradaSincronizada _notaSelecionada; // <-- TIPO CORRIGIDO

        // NOVA PROPRIEDADE: Controla se os botões de manifesto estão habilitados
        public bool IsItemSelected => NotaSelecionada != null;

        // Este método é chamado automaticamente pela CommunityToolkit MVVM
        // sempre que a propriedade NotaSelecionada mudar.
        partial void OnNotaSelecionadaChanged(DfeEntradaSincronizada value)
        {
            // Avisa a interface gráfica que a propriedade IsItemSelected mudou,
            // para que os botões sejam habilitados/desabilitados.
            OnPropertyChanged(nameof(IsItemSelected));
        }

        public DfeMonitorViewModel(IDfeRepository dfeRepository, IEmpresaRepository empresaRepository,
            IRegionManager regionManager,
            IDfeSefazService dfeSefazService,
            ILoadingService loadingService) 
        {
            _dfeRepository = dfeRepository;
            _empresaRepository = empresaRepository;
            _regionManager = regionManager;
            _dfeSefazService = dfeSefazService;
            _loadingService = loadingService;

            _notas = new ObservableCollection<DfeEntradaSincronizada>();
            _empresas = new ObservableCollection<Empresa>();

            CarregarTelaCommand.Execute(null);
        }


        [RelayCommand]
        private async Task ConsultarSefaz()
        {
            if (EmpresaSelecionada == null)
            {
                MessageBox.Show("Por favor, selecione uma empresa primeiro.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            _loadingService.Show("Consultando SEFAZ... Isso pode levar alguns minutos.");
            try
            {
                int novasNotas = await _dfeSefazService.ConsultarDocumentosDestinadosAsync(EmpresaSelecionada.Id, EmpresaSelecionada.Cnpj);
                MessageBox.Show($"{novasNotas} novos documentos foram baixados com sucesso!", "Consulta Concluída", MessageBoxButton.OK, MessageBoxImage.Information);

                // Recarrega a grade para mostrar as novas notas
                await CarregarDados();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro na Consulta", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _loadingService.Hide();
            }
        }



        [RelayCommand]
        private async Task CarregarTela()
        {
            _isInitializing = true; // Ativa a "trava"

            await CarregarEmpresas();

            if (Empresas.Any())
            {
                // Procura na lista pela empresa que corresponde ao ID do usuário logado
                EmpresaSelecionada = Empresas.FirstOrDefault(e => e.Id == SessaoUsuario.EmpresaId);

                // Se, por algum motivo, não encontrar (o que seria raro), seleciona a primeira como fallback
                if (EmpresaSelecionada == null)
                {
                    EmpresaSelecionada = Empresas.FirstOrDefault();
                }

                await CarregarDados();
            }

            _isInitializing = false; // Desativa a "trava"
        }

        partial void OnEmpresaSelecionadaChanged(Empresa value)
        {
            // Agora, este método só recarrega os dados se não estiver na inicialização
            // e se o usuário realmente selecionou uma empresa.
            if (value != null && !_isInitializing)
            {
                CarregarDadosCommand.Execute(null);
            }
        }

        [RelayCommand]
        private async Task CarregarDados()
        {
            if (EmpresaSelecionada == null) return;

            try
            {
                Notas.Clear();
                var notasDoBanco = await _dfeRepository.GetNotasSincronizadasAsync(EmpresaSelecionada.Id);
                foreach (var nota in notasDoBanco)
                {
                    Notas.Add(nota);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao carregar as notas: {ex.Message}", "Erro de Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task CarregarEmpresas()
        {
            try
            {
                Empresas.Clear();
                var listaEmpresas = await _empresaRepository.GetAllAsync();
                foreach (var empresa in listaEmpresas)
                {
                    Empresas.Add(empresa);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro ao carregar a lista de empresas: {ex.Message}", "Erro de Conexão", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        // NOVO COMANDO: Lógica para o clique dos botões de manifesto
        [RelayCommand]
        private async Task Manifestar(TipoEventoManifestacao tipoEvento)
        {
            if (NotaSelecionada == null) return;

            var nomeEvento = tipoEvento.ToString().Replace("Da", " da ").Replace("Nao", " Não ");
            var confirmacao = MessageBox.Show($"Deseja realmente executar '{nomeEvento}' para a nota {NotaSelecionada.NumeroNota}?",
                                               "Confirmação de Manifesto", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (confirmacao == MessageBoxResult.No) return;

            _loadingService.Show("Enviando manifesto para a SEFAZ...");
            try
            {
                // Chamada ao serviço que faz a comunicação com a SEFAZ
                var novoStatus = await _dfeSefazService.EnviarManifestoAsync(EmpresaSelecionada.Id, EmpresaSelecionada.Cnpj, NotaSelecionada.ChaveAcesso, tipoEvento);

                // Atualiza o status na tela imediatamente para o usuário ver
                NotaSelecionada.ManifestoStatus = novoStatus;

                MessageBox.Show("Manifesto enviado com sucesso!", "Sucesso", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ocorreu um erro: {ex.Message}", "Erro no Manifesto", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                _loadingService.Hide();
            }
        }

        [RelayCommand]
        private void LancarEntrada()
        {
            if (NotaSelecionada == null) return;

            // 1. Cria o "pacote de dados" do fornecedor
            FornecedorParaCadastroDto fornecedorDto = ExtrairDadosDoFornecedor(NotaSelecionada);

            // 2. Prepara os parâmetros para a navegação
            var parameters = new NavigationParameters
    {
        { "nota", NotaSelecionada },
        { "dadosFornecedor", fornecedorDto } // Envia o pacote de dados junto com a nota
    };

            // 3. Navega para o dashboard de entrada
            _regionManager.RequestNavigate("ContentRegion", "EntradaDashboardView", parameters);
        }


        private FornecedorParaCadastroDto ExtrairDadosDoFornecedor(DfeEntradaSincronizada nota)
        {
            // Começa com os dados básicos como fallback
            var dto = new FornecedorParaCadastroDto
            {
                Cnpj = nota.CnpjEmitente,
                RazaoSocial = nota.NomeEmitente,
                NomeFantasia = nota.NomeEmitente
            };

            if (string.IsNullOrWhiteSpace(nota.XmlCompleto))
            {
                return dto; // Retorna apenas os dados básicos se não houver XML
            }

            try
            {
                var nfeProc = FuncoesXml.XmlStringParaClasse<NFe.Classes.nfeProc>(nota.XmlCompleto);
                if (nfeProc?.NFe?.infNFe?.emit == null) return dto;

                var emitente = nfeProc.NFe.infNFe.emit;
                var endereco = emitente.enderEmit;

                // Preenche o DTO com todos os dados do XML
                dto.RazaoSocial = emitente.xNome;
                dto.NomeFantasia = !string.IsNullOrWhiteSpace(emitente.xFant) ? emitente.xFant : emitente.xNome;
                dto.InscricaoEstadual = emitente.IE;

                if (endereco != null)
                {
                    dto.Telefone = endereco.fone?.ToString();
                    dto.Cep = endereco.CEP;
                    dto.Logradouro = endereco.xLgr;
                    dto.Numero = endereco.nro;
                    dto.Bairro = endereco.xBairro;
                    dto.Cidade = endereco.xMun;
                    dto.Uf = endereco.UF.ToString();
                    dto.CodigoMunicipio = endereco.cMun;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Erro ao extrair dados do fornecedor do XML: {ex.Message}");
                // Em caso de erro, o DTO com dados básicos é retornado, não quebra a aplicação.
            }

            return dto;
        }




        // --- NOVO COMANDO PARA FECHAR A ABA ---
        [RelayCommand]
        private void FecharAba(UserControl view)
        {
            if (view != null)
            {
                _regionManager.Regions["ContentRegion"].Remove(view);
            }
        }
    }
}