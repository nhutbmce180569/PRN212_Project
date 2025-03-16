using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FinalProject.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using WPFLab.Helper;
using WPFLab.ViewModels;
using FinalProject.Helper;
using FinalProject.Views.ShopManager;
using FinalProject.Views.WarehouseManager;
using FinalProject.Views.OrderManager.Order;
using FinalProject.Views.OrderManager;
using FinalProject.Views.Admin;

namespace FinalProject.ViewModels
{
    public class LoginViewModel : BaseViewModel
    {
        public ICommand LoginCommand { get; }

        public LoginViewModel()
        {
            LoginCommand = new RelayCommand(Login);
        }

        private async void Login(object obj)
        {
            if (!string.IsNullOrWhiteSpace(UsernameBox) && !string.IsNullOrWhiteSpace(PasswordBox))
            {
                Employee em = await Task.Run(() => CheckLogin(_usernameBox, _passwordBox));

                if (em != null)
                {
                    //Hàm đưa Employee vào hệ thống
                    Application.Current.Properties["Employee"] = em;
                    //hàm lấy Employee ra hệ thống
                    //if (Application.Current.Properties.Contains("CurrentEmployee"))
                    //{
                    //    var currentEmployee = Application.Current.Properties["CurrentEmployee"] as Employee;
                    //    if (currentEmployee != null)
                    //    {
                    //        MessageBox.Show($"Welcome, {currentEmployee.Name}");
                    //    }
                    //}
                    if (em.RoleId == 1)
                    {
                        new AdminView().Show();
                    }
                    else if (em.RoleId == 2)
                    {
                        new ShopManagerView().Show();
                        
                    }
                    else if (em.RoleId == 3)
                    {
                        new WarehouseManagerView().Show();
                    }
                    else if (em.RoleId == 4)
                    {
                        new OrderManagerView().Show();
                    }
                    else
                    {
                        MessageBox.Show("Role is not correct!");
                        return;
                    }
                    Application.Current.MainWindow.Close();
                }
                else
                {
                    MessageBox.Show("Login failed!");
                }
            }
            else
            {
                MessageBox.Show("Please enter username and password");
            }
        }


        private Employee CheckLogin(string email, string password)
        {
            using (var context = new FstoreContext())
            {
                var normalizedEmail = email.ToLower();
                var hashedPassword = PasswordBoxHelper.GetMD5(password);

                var employee = context.Employees.FirstOrDefault(
                    c => c.Email.ToLower() == normalizedEmail &&
                         c.Password == hashedPassword
                );
                return employee;
            }
        }

        private string _usernameBox = string.Empty;

        public string UsernameBox
        {
            get { return _usernameBox; }
            set
            {
                _usernameBox = value;
                OnPropertyChanged(nameof(UsernameBox));
            }
        }

        private string _passwordBox = string.Empty;
        public string PasswordBox
        {
            get { return _passwordBox; }
            set
            {
                _passwordBox = value;
                OnPropertyChanged(nameof(PasswordBox));
            }
        }
    }
}
