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
        }

       public void ButtonClick_Customer(object sender, RoutedEventArgs e)
        {
            fmt.Content = new CustomerListView();
        }
    }
}
