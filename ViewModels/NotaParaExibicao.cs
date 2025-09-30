using CommunityToolkit.Mvvm.ComponentModel;
using FluxoSistema.Core.Models;

namespace FluxoSistema.Dfe.ViewModels
{
    // Esta classe "envelopa" sua nota e adiciona a propriedade de seleção
    public partial class NotaParaExibicao : ObservableObject
    {
        // Contém a nota original do banco
        public DfeEntradaSincronizada Nota { get; }

        // Propriedade que o CheckBox da tela vai usar
        [ObservableProperty]
        private bool _estaSelecionada;

        public NotaParaExibicao(DfeEntradaSincronizada nota)
        {
            Nota = nota;
        }
    }
}