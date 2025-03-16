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
using FinalProject.Views.OrderManager.Order;
using FinalProject.Views.Profile;

namespace FinalProject.Views.OrderManager
{
    /// <summary>
    /// Interaction logic for OrderManagerView.xaml
    /// </summary>
    public partial class OrderManagerView : Window
    {
        public OrderManagerView()
        {
            InitializeComponent();
        }

        private void ButtonClick_Orders(object sender, RoutedEventArgs e)
        {
            fmt.Content = new OrderListView();
        }

        private void ButtonClick_Logout(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Are you sure to logout?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
            {
                Application.Current.Properties["Employee"] = null;
                Application.Current.MainWindow?.Close();
                new MainWindow().Show();
                this.Close();
            }
        }
        private void ButtonClick_ShowProfile(object sender, RoutedEventArgs e)
        {
            fmt.Content = new UserProfile();
        }
    }
}
