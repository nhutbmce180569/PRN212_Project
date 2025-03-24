using FinalProject.ViewModels.WarehouseManager;
using System.Windows;
using System.Windows.Controls;

namespace FinalProject.Views.WarehouseManager.ImportOrder
{
    /// <summary>
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class ProductListView : Page
    {
        public ProductListView()
        {
            InitializeComponent();
            DataContext = new ImportProductViewModel();
        }

    }
}
