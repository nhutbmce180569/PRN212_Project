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
using FinalProject.ViewModels.ShopManager;
using FinalProject.Models;

namespace FinalProject.Views.ShopManager.Customer
{
    /// <summary>
    /// Interaction logic for AddCustomer.xaml
    /// </summary>
    public partial class UpdateCustomer : Window
    {
        public UpdateCustomer(FinalProject.Models.Customer customer)
        {
            InitializeComponent();
            txtGender.Items.Add("Male");
            txtGender.Items.Add("Female");
            txtGender.Items.Add("Other");
            txtFullName.Text = customer.FullName;
            txtBirthday.SelectedDate = customer.Birthday;
            txtGender.SelectedIndex = txtGender.Items.IndexOf(customer.Gender.Trim());
            txtEmail.Text = customer.Email;
            txtPassword.Text = customer.Password;
            txtPhoneNumber.Text = customer.PhoneNumber;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;
            Application.Current.Windows[0].IsHitTestVisible = true;

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }
    }
}
