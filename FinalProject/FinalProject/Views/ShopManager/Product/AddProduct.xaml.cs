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
using System.Windows.Shapes;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;

namespace FinalProject.Views.ShopManager.Product
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewBrand_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel viewModel)
            {
                viewModel.AddNewBrand();
            }
        }

        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            if (DataContext is ProductViewModel viewModel)
            {
                viewModel.AddNewCategory();
            }
        }
    }
}
