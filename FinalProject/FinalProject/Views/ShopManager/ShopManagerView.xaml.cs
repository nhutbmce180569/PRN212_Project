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
using FinalProject.Views.Profile;
using FinalProject.Views.ShopManager.Product;

namespace FinalProject.Views.ShopManager
{
    /// <summary>
    /// Interaction logic for ShopManagerView.xaml
    /// </summary>
    public partial class ShopManagerView : Window
    {
        public ShopManagerView()
        {
            InitializeComponent();
            fmt.Content = new CustomerListView();
        }

        public void ButtonClick_Customer(object sender, RoutedEventArgs e)
        {
            fmt.Content = new CustomerListView();
        }
        public void ButtonClick_ShowProfile(object sender, RoutedEventArgs e)
        {
            fmt.Content = new UserProfile();
        }
        public void ButtonClick_Logout(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to logout?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Properties["Employee"] = null;
                Application.Current.MainWindow?.Close();
                new MainWindow().Show();
                this.Close();
            } 
        }
        private void ButtonClick_Product(object sender, RoutedEventArgs e)
        {
            fmt.Content = new ProductListView();
        }
    }
}
