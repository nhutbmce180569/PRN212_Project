using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;
using FinalProject.Views.ShopManager.Customer;
using MaterialDesignColors;
using Newtonsoft.Json;
using WPFLab.Helper;
using WPFLab.ViewModels;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using FinalProject.Helper;
using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;
using FinalProject.Views.Admin.Employee;
using Microsoft.EntityFrameworkCore;
using Microsoft.Win32;

namespace FinalProject.ViewModels.Admin
{
    internal class EmployeeViewModel : BaseViewModel
    {
        public event Action OnEmployeeAdded;

        private ObservableCollection<Employee> _employeeList;
        public ObservableCollection<Employee> EmployeeList
        {
            get => _employeeList;
            set
            {
                _employeeList = value;
                OnPropertyChanged(nameof(EmployeeList)); // Kích hoạt UI update
            }
        }

        public ObservableCollection<Employee> AllEmployeeList { set; get; }

        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }


        public EmployeeViewModel()
        {
            TextBoxItem.CreatedDate = DateTime.Now;
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
            ExportCommand = new RelayCommand(Export);
        }

        private void Export(object obj)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = "employees.xlsx",
                    Title = "Save Exported Data"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var workbook = new ClosedXML.Excel.XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Employees");

                    var headerRange = worksheet.Range(1, 1, 1, 9);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    worksheet.Columns().AdjustToContents();

                    // Header row
                    worksheet.Cell(1, 1).Value = "Employee ID";
                    worksheet.Cell(1, 2).Value = "FullName";
                    worksheet.Cell(1, 3).Value = "Birthday";
                    worksheet.Cell(1, 4).Value = "Password";
                    worksheet.Cell(1, 5).Value = "Phone Number";
                    worksheet.Cell(1, 6).Value = "Email";
                    worksheet.Cell(1, 7).Value = "Gender";
                    worksheet.Cell(1, 8).Value = "CreatedDate";
                    worksheet.Cell(1, 9).Value = "Status";
                    worksheet.Cell(1, 10).Value = "Role";

                    // Data rows
                    for (int i = 0; i < AllEmployeeList.Count; i++)
                    {
                        var p = AllEmployeeList[i];
                        worksheet.Cell(i + 2, 1).Value = p.EmployeeId;
                        worksheet.Cell(i + 2, 2).Value = p.FullName;
                        worksheet.Cell(i + 2, 3).Value = p.Birthday;
                        worksheet.Cell(i + 2, 4).Value = p.Password;
                        worksheet.Cell(i + 2, 5).Value = p.PhoneNumber;
                        worksheet.Cell(i + 2, 6).Value = p.Email;
                        worksheet.Cell(i + 2, 7).Value = p.Gender;
                        worksheet.Cell(i + 2, 8).Value = p.CreatedDate;
                        worksheet.Cell(i + 2, 9).Value = p.Status;
                        if (p.RoleId == 1)
                        {
                            worksheet.Cell(i + 2, 10).Value = "Admin";
                        }
                        else if (p.RoleId == 2)
                        {
                            worksheet.Cell(i + 2, 10).Value = "Shop Manager";
                        }
                        else if (p.RoleId == 3)
                        {
                            worksheet.Cell(i + 2, 10).Value = "Warehouse Manager";
                        }
                        else if (p.RoleId == 4)
                        {
                            worksheet.Cell(i + 2, 10).Value = "Order Manager";
                        }

                    }

                    workbook.SaveAs(saveFileDialog.FileName);
                    MessageBox.Show("Export to Excel successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting to Excel: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public EmployeeViewModel(Employee employee)
        {
            SelectedItem = employee;
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
        }


        private Employee textBoxItem = new Employee();
        public Employee TextBoxItem
        {
            get { return textBoxItem; }
            set
            {
                textBoxItem = value;
                OnPropertyChanged(nameof(TextBoxItem));
            }
        }

        private Employee selectedItem;
        public Employee SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (selectedItem != null)
                {
                    textBoxItem = JsonConvert.DeserializeObject<Employee>(JsonConvert.SerializeObject(selectedItem));
                    OnPropertyChanged(nameof(TextBoxItem));
                }
            }
        }


        private void OpenPopup(object obj)
        {
            var action = new EmployeeViewModel();
            action.OnEmployeeAdded += Load;
            var popup = new AddEmployee
            {
                DataContext = action,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner

            };
            // Dùng Dispatcher để tránh lỗi "Window is closing"
            popup.Deactivated += (s, e) =>
            {
                Application.Current.Windows[0].IsHitTestVisible = false;
                if (popup.IsLoaded)
                {

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!popup.IsActive) // khi click chuột ra ngoài không trong popup
                        {
                            popup.Topmost = true;

                            //// Lấy lại focus cho MainWindow
                            //Application.Current.MainWindow.Opacity = 1;
                        }
                    }));
                }
            };
            Application.Current.Windows[0].Opacity = 0.5;
            popup.ShowDialog();
        }
        private void OpenUpdatePopup(object obj)
        {
            if (selectedItem != null)
            {
                var action = new EmployeeViewModel(selectedItem);
                action.OnEmployeeAdded += Load;
                var popup = new UpdateEmployee(selectedItem)
                {
                    DataContext = action,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                // Dùng Dispatcher để tránh lỗi "Window is closing"
                popup.Deactivated += (s, e) =>
                {
                    if (popup.IsLoaded)
                    {
                        Application.Current.Windows[0].IsHitTestVisible = false;

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!popup.IsActive) // khi click chuột ra ngoài không trong popup
                            {
                                popup.Topmost = true;

                                //// Lấy lại focus cho MainWindow
                                //Application.Current.MainWindow.Activate();
                                //Application.Current.MainWindow.Opacity = 1;

                            }
                        }));
                    }
                };
                Application.Current.Windows[0].Opacity = 0.5;
                popup.ShowDialog();
            }
            else
            {
                MessageBox.Show("Please select customer to update", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }



        public void Load()
        {
            using (var context = new FstoreContext())
            {
                var list = context.Employees.Where(e => e.RoleId != 1).ToList();
                AllEmployeeList = new ObservableCollection<Employee>(list);
                EmployeeList = new ObservableCollection<Employee>(AllEmployeeList);
            }
        }


        private void Add(Object obj)
        {
            // Kiểm tra các trường bắt buộc
            if (textBoxItem.FullName.IsNullOrEmpty() ||
                textBoxItem.Gender.IsNullOrEmpty() ||
                textBoxItem.Email.IsNullOrEmpty() ||
                textBoxItem.PhoneNumber.IsNullOrEmpty() ||
                textBoxItem.Password.IsNullOrEmpty() ||
                textBoxItem.Status.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (!IsValidEmail(textBoxItem.Email))
            {
                MessageBox.Show("Invalid email format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!IsValidPhoneNumber(textBoxItem.PhoneNumber))
            {
                MessageBox.Show("Phone number can only be entered in numbers and up to 15 characters", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if(textBoxItem.Birthday > DateTime.Now)
            {
                MessageBox.Show("Birthday cannot be in the future", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (IsEmailExists(textBoxItem.Email))
            {
                MessageBox.Show("Email already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!IsValidPassword(textBoxItem.Password))
            {
                MessageBox.Show("Password must be at least 8 characters long, contain at least one uppercase letter and one special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var item = new Employee
                {
                    FullName = textBoxItem.FullName,
                    Gender = textBoxItem.Gender,
                    Email = textBoxItem.Email,
                    PhoneNumber = textBoxItem.PhoneNumber,
                    Birthday = textBoxItem.Birthday,
                    Password = PasswordBoxHelper.GetMD5(textBoxItem.Password),
                    CreatedDate = DateTime.Now,
                    Status = textBoxItem.Status,
                    RoleId = textBoxItem.RoleId
                };

                using (var context = new FstoreContext())
                {
                    context.Employees.Add(item);
                    context.SaveChanges();
                }

                OnEmployeeAdded?.Invoke();
                textBoxItem = new Employee();
                OnPropertyChanged(nameof(TextBoxItem));
                Application.Current.Windows[2]?.Close();
                Application.Current.Windows[0].Opacity = 1;
                Application.Current.MainWindow.Focus();
                Application.Current.Windows[0].IsHitTestVisible = true;
                MessageBox.Show("Add Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }

           
        }


        private void Update(object obj)
        {
            if (textBoxItem.FullName.IsNullOrEmpty() ||
                textBoxItem.Gender.IsNullOrEmpty() ||
                textBoxItem.Email.IsNullOrEmpty() ||
                textBoxItem.PhoneNumber.IsNullOrEmpty() ||
                textBoxItem.Password.IsNullOrEmpty() ||
                textBoxItem.Status.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else if (!IsValidPhoneNumber(textBoxItem.PhoneNumber))
            {
                MessageBox.Show("Phone number can only be entered in numbers and up to 15 characters!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (textBoxItem.Birthday > DateTime.Now)
            {
                MessageBox.Show("Birthday cannot be in the future", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (textBoxItem.Password != selectedItem.Password)
            {
                if (!IsValidPassword(textBoxItem.Password))
                {
                    MessageBox.Show("Password must be at least 8 characters long, contain at least one uppercase letter and one special character",
                                    "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                TextBoxItem.Password = PasswordBoxHelper.GetMD5(textBoxItem.Password);
            }
            else if (!CheckEmail(textBoxItem.Email, selectedItem.Email))
            {
                return;
            }
            else
            {
                using (var context = new FstoreContext())
                {
                    context.Employees.Update(textBoxItem);
                    context.SaveChanges();
                }
                OnEmployeeAdded?.Invoke();
                textBoxItem = new Employee();
                OnPropertyChanged(nameof(TextBoxItem));
                Application.Current.Windows[2]?.Close();
                Application.Current.Windows[0].Opacity = 1;
                Application.Current.Windows[0].Focus();
                Application.Current.Windows[0].IsHitTestVisible = true;
                MessageBox.Show("Update Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }          
        }
        

        private void Delete(object obj)
        {
            if (selectedItem == null)
            {
                MessageBox.Show("Please select customer to delete", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete this Employee?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (var context = new FstoreContext())
                    {
                        var employee = context.Employees.FirstOrDefault(e => e.EmployeeId == selectedItem.EmployeeId);
                        if (employee.Status == "Active")
                        {
                            MessageBox.Show("The employee is in active status!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            context.Employees.Remove(employee);
                            context.SaveChanges();
                            EmployeeList.Remove(selectedItem);
                            AllEmployeeList.Remove(selectedItem);
                            MessageBox.Show("Delete Successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }


        private String searchBoxItem;
        public String SearchBoxItem
        {
            get { return searchBoxItem; }
            set
            {
                searchBoxItem = value;
                OnPropertyChanged(nameof(SearchBoxItem));
            }
        }
        private void Search(object obj)
        {
            if (!searchBoxItem.Equals(""))
            {
                var list = from item in AllEmployeeList
                           where item.FullName.ToLower().Contains(searchBoxItem.ToLower())
                           select item;
                if (list.Any())
                {
                    EmployeeList = new ObservableCollection<Employee>(list);
                }
                else
                {
                    MessageBox.Show("There is no employee", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (searchBoxItem.Equals(""))
            {
                EmployeeList = new ObservableCollection<Employee>(AllEmployeeList);
                OnPropertyChanged(nameof(EmployeeList));
            }
        }


        private bool CheckEmail(string newEmail, string originalEmail)
        {
            // Nếu email không thay đổi, không cần kiểm tra gì thêm
            if (newEmail.Equals(originalEmail, StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }

            // Kiểm tra định dạng email
            if (!IsValidEmail(newEmail))
            {
                MessageBox.Show("Email is not in correct format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Kiểm tra email đã tồn tại trong cơ sở dữ liệu chưa
            using (var context = new FstoreContext())
            {
                bool emailExists = context.Employees.Any(e => e.Email.ToLower() == newEmail.ToLower());
                if (emailExists)
                {
                    MessageBox.Show("Email already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }

            // Email hợp lệ và chưa tồn tại
            return true;
        }


        public bool IsEmailExists(string email)
        {
            using (var context = new FstoreContext())
            {
                return context.Employees.Any(e => e.Email.ToLower() == email.ToLower());
            }
        }

        public bool IsValidPassword(string password)
        {
            if (string.IsNullOrWhiteSpace(password))
                return false;
            string pattern = @"^(?=.*[A-Z])(?=.*[^a-zA-Z0-9]).{8,}$";
            return Regex.IsMatch(password, pattern);
        }

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (string.IsNullOrWhiteSpace(phoneNumber))
                return false;
            string pattern = @"^\d{1,15}$"; // Chỉ cho phép nhập số và tối đa 15 ký tự
            return Regex.IsMatch(phoneNumber, pattern);
        }
    }
}
