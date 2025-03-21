﻿using System;
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
using FinalProject.ViewModels;

namespace FinalProject.Views.Profile
{
    /// <summary>
    /// Interaction logic for UpdateProfilePopup.xaml
    /// </summary>
    public partial class UpdateProfilePopup : Window
    {
        public UpdateProfilePopup()
        {
            InitializeComponent();
        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        { 
            Close();
            Application.Current.MainWindow.IsHitTestVisible = true;
            Application.Current.Windows[0].Opacity = 1;

        }
    }
}
