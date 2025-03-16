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

        public EmployeeViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
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
                var list = context.Employees.ToList();
                AllEmployeeList = new ObservableCollection<Employee>(list);
                EmployeeList = new ObservableCollection<Employee>(AllEmployeeList);
            }               
        }

        private void Add(Object obj)
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
            else
            {
                if (IsValidEmail(textBoxItem.Email))
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
                else
                {
                    MessageBox.Show("Invalid email format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }

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
            else
            {
                TextBoxItem.Password = PasswordBoxHelper.GetMD5(textBoxItem.Password);
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
                            MessageBox.Show("Employee Active!!!!!", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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

        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
