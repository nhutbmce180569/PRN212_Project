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
    /// Interaction logic for ProductListView.xaml
    /// </summary>
    public partial class ProductListView : Page
    {
        public ProductListView()
        {
            InitializeComponent();
            DataContext = new ProductViewModel();
        }

        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is int productId)
            {
                if (DataContext is ProductViewModel vm)
                {
                    vm.SelectedProducts[productId] = true;
                }
            }
        }

        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
        {
            if (sender is CheckBox cb && cb.Tag is int productId)
            {
                if (DataContext is ProductViewModel vm)
                {
                    vm.SelectedProducts[productId] = false;
                }
            }
        }

    }
}
