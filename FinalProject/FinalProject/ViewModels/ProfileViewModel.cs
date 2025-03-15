using System;
using System.Windows;
using System.Windows.Input;
using FinalProject.Helper;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;
using FinalProject.Views.Profile;
using Microsoft.IdentityModel.Tokens;
using WPFLab.Helper;
using WPFLab.ViewModels;

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

            // Kiểm tra các ô nhập mật khẩu có bị rỗng không
            if (string.IsNullOrEmpty(_oldPasswordBox) ||
                string.IsNullOrEmpty(_newPasswordBox) ||
                string.IsNullOrEmpty(_confirmPasswordBox))
            {
                MessageBox.Show("Password fields cannot be empty!");
                return;
            }

            // Kiểm tra mật khẩu mới có trùng với xác nhận mật khẩu không
            if (_newPasswordBox != _confirmPasswordBox)
            {
                MessageBox.Show("New password and confirm password do not match!");
                return;
            }

            // Kiểm tra độ mạnh của mật khẩu mới
            if (!IsValidPassword(_newPasswordBox))
            {
                MessageBox.Show("New password does not meet security requirements!");
                return;
            }

            // Mã hóa mật khẩu cũ và kiểm tra với mật khẩu hiện tại trong database
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

        private bool IsValidPassword(string password)
        {
            // Kiểm tra mật khẩu có ít nhất 8 ký tự, bao gồm chữ hoa, chữ thường, số và ký tự đặc biệt
            return password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
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
                // Kiểm tra dữ liệu đầu vào
                if (string.IsNullOrEmpty(EmployeeView.FullName) ||
                    string.IsNullOrEmpty(EmployeeView.PhoneNumber) ||
                    string.IsNullOrEmpty(EmployeeView.Gender))
                {
                    MessageBox.Show("Update Employee failed: Missing required fields");
                    return;
                }

                using (var context = new FstoreContext())
                {
                    // Kiểm tra context có hoạt động không
                    if (context == null)
                    {
                        MessageBox.Show("Database context could not be initialized");
                        return;
                    }

                    // Lưu thông tin vào Application Properties
                    Application.Current.Properties["Employee"] = EmployeeUpdate;



                    // Cập nhật giao diện
                    OnPropertyChanged(nameof(EmployeeUpdate));

                    // Tìm nhân viên cần cập nhật
                    var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == EmployeeUpdate.EmployeeId);

                    if (employee != null)
                    {

                        // Cập nhật thông tin nhân viên
                        employee.FullName = EmployeeUpdate.FullName;
                        employee.PhoneNumber = EmployeeUpdate.PhoneNumber;
                        employee.Birthday = EmployeeUpdate.Birthday;
                        employee.Gender = EmployeeUpdate.Gender;

                        // Lưu thay đổi vào database
                        context.SaveChanges();

                        // 🔥 Gán một instance mới để kích hoạt OnPropertyChanged
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
                        // Đóng popup sau khi thay đổi mật khẩu thành công
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
