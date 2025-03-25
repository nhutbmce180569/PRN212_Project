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

namespace FinalProject.Views.Admin.Employee
{
    /// <summary>
    /// Interaction logic for UpdateEmployee.xaml
    /// </summary>
    public partial class UpdateEmployee : Window
    {
        public UpdateEmployee()
        {
            InitializeComponent();
        
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;
            Application.Current.Windows[0].IsHitTestVisible = true;

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove();
        }
    }
}
