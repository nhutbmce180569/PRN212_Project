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

namespace FinalProject.Views.Profile
{
    /// <summary>
    /// Interaction logic for ChangePasswordPopup.xaml
    /// </summary>
    public partial class ChangePasswordPopup : Window
    {
        public ChangePasswordPopup()
        {
            InitializeComponent();
        }
        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.IsHitTestVisible = true;
            Application.Current.Windows[0].Opacity = 1;

            Close();
        }
    }
}
