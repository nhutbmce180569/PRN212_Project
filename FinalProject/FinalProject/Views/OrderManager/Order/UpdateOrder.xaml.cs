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
    /// Interaction logic for UpdateOrder.xaml
    /// </summary>
    public partial class UpdateOrder : Window
    {
        public UpdateOrder(FinalProject.Models.Order Order)
        {
            InitializeComponent();
            DataContext = new OrderViewModel();
        }

        private void Button_Clicku(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;
        }

        private void Border_MouseLeftButtonDownu(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }
    }
}
