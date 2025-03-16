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
using FinalProject.ViewModels.OrderManager;

namespace FinalProject.Views.OrderManager.Order
{
    /// <summary>
    /// Interaction logic for AddOrder.xaml
    /// </summary>
    public partial class AddOrder : Window
    {
        public AddOrder()
        {
            InitializeComponent();
            DataContext = new OrderViewModel();
        }

        private void Button_Clicko(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;

        }
        private void Border_MouseLeftButtonDowno(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }
    }
}
