using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FluxoSistema.Dfe.Views
{
    /// <summary>
    /// Interação lógica para DfeMonitorView.xam
    /// </summary>
    public partial class DfeMonitorView : UserControl
    {
        public DfeMonitorView()
        {
            InitializeComponent();
        }

        private void DataGridCell_PreviewMouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (sender is DataGridCell cell && cell.Content is CheckBox checkBox)
            {
                checkBox.IsChecked = !checkBox.IsChecked;
                e.Handled = true;
            }
        }
    }
}
