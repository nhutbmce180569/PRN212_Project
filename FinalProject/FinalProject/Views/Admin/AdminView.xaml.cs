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
using FinalProject.ViewModels.Admin;
using FinalProject.Views.Admin.Employee;
using FinalProject.Views.Profile;
using FinalProject.Views.ShopManager;

namespace FinalProject.Views.Admin
{
    /// <summary>
    /// Interaction logic for AdminView.xaml
    /// </summary>
    public partial class AdminView : Window
    {
        public AdminView()
        {
            InitializeComponent();
            fmt.Content = new EmployeeListView();
        }

        private void ButtonClick_Employee(object sender, RoutedEventArgs e)
        {
            fmt.Content = new EmployeeListView();
        }

        public void ButtonClick_ShowProfile(object sender, RoutedEventArgs e)
        {
            fmt.Content = new UserProfile();
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
    }
}
