using FinalProject.ViewModels.WarehouseManager;
using System.Windows.Controls;

namespace FinalProject.Views.WarehouseManager.Supplier
{
    /// <summary>
    /// Interaction logic for SupplierListView.xaml
    /// </summary>
    public partial class SupplierListView : Page
    {
        public SupplierListView()
        {
            InitializeComponent();
            searchBox.Text = "Search by name";
            DataContext = new SupplierViewModel();
        }
    }
}
