using FinalProject.ViewModels.WarehouseManager;
using System.Windows.Controls;

namespace FinalProject.Views.WarehouseManager.ImportOrder
{
    /// <summary>
    /// Interaction logic for ImportOrderListView.xaml
    /// </summary>
    public partial class ImportOrderListView : Page
    {
        public ImportOrderListView()
        {
            InitializeComponent();
            DataContext = new ImportOrderViewModel();
        }

        private void Button_Click_Detail(object sender, System.Windows.RoutedEventArgs e)
        {

        }
    }
}
