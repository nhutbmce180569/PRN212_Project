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
using FinalProject.ViewModels.ShopManager;

namespace FinalProject.Views.ShopManager.Product
{
    /// <summary>
    /// Interaction logic for CategoryListView.xaml
    /// </summary>
    public partial class CategoryListView : Page
    {
        public CategoryListView()
        {
            InitializeComponent();
            DataContext = new CategoryViewModel();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            // Điều hướng quay lại trang trước (ProductListView)
            NavigationService.GoBack();
        }
    }
}
