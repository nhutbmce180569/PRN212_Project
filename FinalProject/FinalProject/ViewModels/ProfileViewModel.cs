﻿using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using FinalProject.Helper;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;
using FinalProject.Views.Profile;
using Microsoft.IdentityModel.Tokens;
using WPFLab.Helper;
using WPFLab.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalProject.ViewModels
{
    public class ProfileViewModel : BaseViewModel
    {
        public event Action OnCustomerAdded;
        public List<string> GenderList { get; } = new List<string> { "Male", "Female", "Other" };

        public ICommand UpdatePopupCommand { get; }
        public ICommand ChangePasswordPopupCommand { get; }
        public ICommand SaveChangePasswordCommand { get; }
        public ICommand SaveUpdateCommand { get; }

        private Employee _employeeView;

        public Employee EmployeeView
        {
            get => _employeeView;
            set
            {
                _employeeView = value;
                Console.WriteLine("OnPropertyChanged called for EmployeeView");
                OnPropertyChanged(nameof(EmployeeView));
            }
        }

        private Employee _employeeUpadte;

        public Employee EmployeeUpdate
        {
            get => _employeeUpadte;
            set
            {
                _employeeUpadte = value;
                Console.WriteLine("OnPropertyChanged called for EmployeeUpdate");
                OnPropertyChanged(nameof(EmployeeUpdate));
            }
        }

        private string _oldPasswordBox;

        public string OldPasswordBox
        {
            get => _oldPasswordBox;
            set
            {
                _oldPasswordBox = value;
                Console.WriteLine("OnPropertyChanged called for EmployeeUpdate");
                OnPropertyChanged(nameof(OldPasswordBox));
            }
        }

        private string _newPasswordBox;

        public string NewPasswordBox
        {
            get => _newPasswordBox;
            set
            {
                _newPasswordBox = value;
                Console.WriteLine("OnPropertyChanged called for EmployeeUpdate");
                OnPropertyChanged(nameof(NewPasswordBox));
            }
        }

        private string _confirmPasswordBox;

        public string ConfirmPasswordBox
        {
            get => _confirmPasswordBox;
            set
            {
                _confirmPasswordBox = value;
                Console.WriteLine("OnPropertyChanged called for EmployeeUpdate");
                OnPropertyChanged(nameof(ConfirmPasswordBox));
            }
        }

        public ProfileViewModel()
        {
            Load();
            SaveChangePasswordCommand = new RelayCommand(ChangePassword);
            ChangePasswordPopupCommand = new RelayCommand(OpenChangePasswordPopup);
            SaveUpdateCommand = new RelayCommand(SaveUpdate);
            UpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
        }

        private void ChangePassword(object obj)
        {
            int id = EmployeeView.EmployeeId;
            Validator va = new Validator();

            if (string.IsNullOrEmpty(_oldPasswordBox) ||
                string.IsNullOrEmpty(_newPasswordBox) ||
                string.IsNullOrEmpty(_confirmPasswordBox))
            {
                MessageBox.Show("Password fields cannot be empty!");
                return;
            }

            if (!_newPasswordBox.Equals(_confirmPasswordBox))
            {
                MessageBox.Show("New password and confirm password do not match!");
                return;
            }

            if (_newPasswordBox.Equals(_oldPasswordBox))
            {
                MessageBox.Show("New password and old password must be different!");
                return;
            }

            if (!va.IsValidPassword(_newPasswordBox))
            {
                MessageBox.Show("New password must contain lowercase letters, uppercase letters, numbers and special characters!");
                return;
            }



            

            string oldPasswordHash = PasswordBoxHelper.GetMD5(_oldPasswordBox);

            using (var context = new FstoreContext())
            {
                Employee employee = context.Employees.FirstOrDefault(e => e.EmployeeId == id);

                if (employee == null)
                {
                    MessageBox.Show("Employee not found!");
                    return;
                }

                if (employee.Password != oldPasswordHash)
                {
                    MessageBox.Show("Old password is incorrect!");
                    return;
                }

                // Cập nhật mật khẩu mới
                string newPasswordHash = PasswordBoxHelper.GetMD5(_newPasswordBox);
                employee.Password = newPasswordHash;

                context.SaveChanges();

                // Đóng popup sau khi thay đổi mật khẩu thành công
                if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is ChangePasswordPopup) is Window popup)
                {
                    popup.Close();
                    Application.Current.MainWindow.IsHitTestVisible = true;
                    Application.Current.MainWindow?.Activate();
                    Application.Current.Windows[0].Opacity = 1;

                }
                MessageBox.Show("Password changed successfully!");
            }
        }

        


        private void OpenChangePasswordPopup(object obj)
        {
            if (EmployeeView != null)
            {
                var action = new ProfileViewModel();
                action.OnCustomerAdded += Load;
                var popup = new ChangePasswordPopup()
                {
                    DataContext = action,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                popup.Deactivated += (s, e) =>
                {
                    if (popup.IsLoaded)
                    {
                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            Application.Current.MainWindow.IsHitTestVisible = false;
                            if (!popup.IsActive)
                            {
                                popup.Topmost = true;
                                //popup.Close();
                                //Application.Current.MainWindow?.Activate();
                            }
                        }));
                    }
                };
                Application.Current.Windows[0].Opacity = 0.5;

                popup.Show();
            }
        }


        private void SaveUpdate(object obj)
        {
            try
            {
                Validator va = new Validator();
                if (string.IsNullOrEmpty(EmployeeView.FullName) ||
                    string.IsNullOrEmpty(EmployeeView.PhoneNumber) ||
                    string.IsNullOrEmpty(EmployeeView.Gender))
                {
                    MessageBox.Show("Update Employee failed: Missing required fields");
                    return;
                }

                if (!va.IsValidPhone(EmployeeUpdate.PhoneNumber))
                {
                    MessageBox.Show("Phone number must contain only digits and be between 1 and 11 characters long.");
                    return;
                }
                if (!va.IsValidBirthday((DateTime)EmployeeUpdate.Birthday))
                {
                    MessageBox.Show("Invalid birthday!");
                    return;
                }

                using (var context = new FstoreContext())
                {
                    if (context == null)
                    {
                        MessageBox.Show("Database context could not be initialized");
                        return;
                    }

                    Application.Current.Properties["Employee"] = EmployeeUpdate;



                    OnPropertyChanged(nameof(EmployeeUpdate));

                    var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == EmployeeUpdate.EmployeeId);

                    if (employee != null)
                    {

                        employee.FullName = EmployeeUpdate.FullName;
                        employee.PhoneNumber = EmployeeUpdate.PhoneNumber;
                        employee.Birthday = EmployeeUpdate.Birthday;
                        employee.Gender = EmployeeUpdate.Gender;

                        context.SaveChanges();

                        EmployeeView = new Employee
                        {
                            EmployeeId = employee.EmployeeId,
                            FullName = employee.FullName,
                            PhoneNumber = employee.PhoneNumber,
                            Birthday = employee.Birthday,
                            Gender = employee.Gender
                        };

                        OnCustomerAdded?.Invoke();
                        OnPropertyChanged(nameof(EmployeeView));
                        if (Application.Current.Windows.OfType<Window>().FirstOrDefault(w => w is UpdateProfilePopup) is Window popup)
                        {
                            popup.Close();
                            Application.Current.MainWindow.IsHitTestVisible = true;
                            Application.Current.MainWindow?.Activate();
                            Application.Current.Windows[0].Opacity = 1;
                        }
                        MessageBox.Show("Update Employee success");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }




        private void OpenUpdatePopup(object obj)
        {
            //var popup = new UpdateProfilePopup
            //{
            //    Owner = Application.Current.MainWindow,
            //    WindowStartupLocation = WindowStartupLocation.CenterOwner
            //};

            var action = new ProfileViewModel();
            action.OnCustomerAdded += Load;
            var popup = new UpdateProfilePopup()
            {
                DataContext = action,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            popup.Deactivated += (s, e) =>
            {
                Application.Current.MainWindow.IsHitTestVisible = false;
                if (popup.IsLoaded)
                {
                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!popup.IsActive)
                        {
                            popup.Topmost = true;
                            //popup.Close();
                            //Application.Current.MainWindow?.Activate();
                        }
                    }));
                }
            };

            EmployeeUpdate = new Employee
            {
                EmployeeId = EmployeeView.EmployeeId,
                FullName = EmployeeView.FullName,
                PhoneNumber = EmployeeView.PhoneNumber,
                Birthday = EmployeeView.Birthday,
                Gender = EmployeeView.Gender
            };
            OnPropertyChanged(nameof(EmployeeUpdate));
            Application.Current.Windows[0].Opacity = 0.5;

            popup.Show();
        }

        public void Load()
        {
            if (Application.Current?.Properties.Contains("Employee") == true)
            {
                if (Application.Current.Properties["Employee"] is Employee currentEmployee)
                {
                    using (var context = new FstoreContext())
                    {
                        var employee = context.Employees.FirstOrDefault(
                            c => c.EmployeeId == currentEmployee.EmployeeId
                        );

                        EmployeeView = employee ?? new Employee();
                        EmployeeUpdate = new Employee
                        {
                            EmployeeId = EmployeeView.EmployeeId,
                            FullName = EmployeeView.FullName.Trim(),
                            PhoneNumber = EmployeeView.PhoneNumber.Trim(),
                            Birthday = EmployeeView.Birthday,
                            Gender = EmployeeView.Gender.Trim()
                        };
                        OnPropertyChanged(nameof(EmployeeUpdate));
                        OnPropertyChanged(nameof(EmployeeView));
                    }
                }
                else
                {
                    MessageBox.Show("Employee object is null!");
                }
            }
            else
            {
                MessageBox.Show("No Employee found in Application.Current.Properties!");
            }
        }
    }
}
