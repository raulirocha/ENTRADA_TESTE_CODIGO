// Em: FluxoSistema.Entrada/Views/EntradaDashboardView.xaml.cs
using Prism.Regions;
using System.Windows.Controls;

namespace FluxoSistema.Entrada.Views
{
    // A classe agora implementa a interface INavigationAware
    public partial class EntradaDashboardView : UserControl, INavigationAware
    {
        public EntradaDashboardView()
        {
            InitializeComponent();
        }

        // ESTE MÉTODO É A CHAVE: Ao retornar 'false', ele força o Prism a criar
        // uma nova aba toda vez, em vez de reutilizar uma existente.
        public bool IsNavigationTarget(NavigationContext navigationContext) => false;

        // Estes dois métodos são necessários pela interface, podemos deixar vazios por enquanto.
        public void OnNavigatedFrom(NavigationContext navigationContext)
        {
        }

        public void OnNavigatedTo(NavigationContext navigationContext)
        {
        }
    }
}