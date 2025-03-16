using FinalProject.Views.Profile;
using FinalProject.Views.ShopManager;
using FinalProject.Views.WarehouseManager.Supplier;
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

namespace FinalProject.Views.WarehouseManager
{
    /// <summary>
    /// Interaction logic for WarehouseManagerView.xaml
    /// </summary>
    public partial class WarehouseManagerView : Window
    {
        public WarehouseManagerView()
        {
            InitializeComponent();
        }
        public void ButtonClick_ShowProfile(object sender, RoutedEventArgs e)
        {
            fmt.Content = new UserProfile();
        }
        public void ButtonClick_Supplier(object sender, RoutedEventArgs e)
        {
            fmt.Content = new SupplierListView();
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
    }
}
