using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Intrinsics.X86;
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
using FinalProject.ViewModels.ShopManager;

namespace FinalProject.Views.ShopManager.Product
{
    /// <summary>
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class ProductListView : Page
    {
        public ProductListView()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
            var productViewModel = new ProductViewModel();
        }

        private void NavigateToCategory(object sender, RoutedEventArgs e)
        {
            // Kiểm tra NavigationService có hợp lệ hay không
            if (NavigationService != null)
            {
                NavigationService.Navigate(new CategoryListView());
            }
        }

        // Điều hướng đến trang BrandListView khi nhấn nút
        private void NavigateToBrand(object sender, RoutedEventArgs e)
        {
            if (NavigationService != null)
            {
                NavigationService.Navigate(new BrandListView());
            }
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            var productViewModel = DataContext as ProductViewModel;
            productViewModel?.RefreshData();  // Gọi phương thức Load để tải lại dữ liệu
        }
    }
}
