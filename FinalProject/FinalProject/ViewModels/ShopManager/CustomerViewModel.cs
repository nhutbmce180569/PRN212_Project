using FinalProject.Helper;
using FinalProject.Models;
using FinalProject.Views.ShopManager.Customer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPFLab.Helper;
using WPFLab.ViewModels;

namespace FinalProject.ViewModels.ShopManager
{
    internal class CustomerViewModel : BaseViewModel
    {
        public event Action OnCustomerAdded;

        private ObservableCollection<Customer> _customerList;
        public ObservableCollection<Customer> CustomerList
        {
            get => _customerList;
            set
            {
                _customerList = value;
                OnPropertyChanged(nameof(CustomerList)); // Kích hoạt UI update
            }
        }
        public ObservableCollection<Customer> AllCustomerList { set; get; }
        public ObservableCollection<string> Gender { set; get; } = new ObservableCollection<string>() { "Male", "Female", "Other" };

        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }
        public CustomerViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
            ExportCommand = new RelayCommand(Export);
        }

        public CustomerViewModel(Customer customer)
        {
            SelectedItem = customer;
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
        }

        private Customer textBoxItem = new Customer();
        public Customer TextBoxItem
        {
            get { return textBoxItem; }
            set
            {
                textBoxItem = value;
                OnPropertyChanged(nameof(TextBoxItem));
            }
        }

        private Customer selectedItem;
        public Customer SelectedItem
        {
            get { return selectedItem; }
            set
            {
                selectedItem = value;
                OnPropertyChanged(nameof(SelectedItem));
                if (selectedItem != null)
                {
                    textBoxItem = JsonConvert.DeserializeObject<Customer>(JsonConvert.SerializeObject(selectedItem));
                    TextBoxItem.Gender = selectedItem.Gender.Trim();
                    OnPropertyChanged(nameof(TextBoxItem));
                }
            }
        }



        private void OpenPopup(object obj)
        {
            var action = new CustomerViewModel();
            action.OnCustomerAdded += Load;
            var popup = new AddCustomer
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
                        if (!popup.IsActive) // Check fix loix
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
                var action = new CustomerViewModel(selectedItem);
                action.OnCustomerAdded += Load;
                var popup = new UpdateCustomer()
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

        private void Export(object obj)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = "customers.xlsx",
                    Title = "Save Exported Data"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var workbook = new ClosedXML.Excel.XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Customers");

                    var headerRange = worksheet.Range(1, 1, 1, 9);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    worksheet.Columns().AdjustToContents();

                    // Header row
                    worksheet.Cell(1, 1).Value = "CustomerId";
                    worksheet.Cell(1, 2).Value = "FullName";
                    worksheet.Cell(1, 3).Value = "Email";
                    worksheet.Cell(1, 4).Value = "Gender";
                    worksheet.Cell(1, 5).Value = "Birthday";
                    worksheet.Cell(1, 6).Value = "CreatedDate";
                    worksheet.Cell(1, 7).Value = "PhoneNumber";
                    worksheet.Cell(1, 8).Value = "IsBlock";

                    // Data rows
                    for (int i = 0; i < AllCustomerList.Count; i++)
                    {
                        var p = AllCustomerList[i];
                        worksheet.Cell(i + 2, 1).Value = p.CustomerId;
                        worksheet.Cell(i + 2, 2).Value = p.FullName ?? "";
                        worksheet.Cell(i + 2, 3).Value = p.Email ?? "";
                        worksheet.Cell(i + 2, 4).Value = p.Gender;
                        worksheet.Cell(i + 2, 5).Value = p.Birthday;
                        worksheet.Cell(i + 2, 6).Value = p.CreatedDate;
                        worksheet.Cell(i + 2, 7).Value = p.PhoneNumber;
                        worksheet.Cell(i + 2, 8).Value = p.IsBlock == true ? "Yes" : "No";
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

        public void Load()
        {
            using (var context = new FstoreContext())
            {
                var list = context.Customers.ToList();
                AllCustomerList = new ObservableCollection<Customer>(list);
                CustomerList = new ObservableCollection<Customer>(AllCustomerList);
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
                if (MessageBox.Show("Are you sure to delete this customer?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    using (var context = new FstoreContext())
                    {
                        var list = from item in context.Orders
                                   where item.CustomerId == textBoxItem.CustomerId
                                   select item;
                        if (list.Any())
                        {
                            MessageBox.Show("This Customer is having order", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                        else
                        {
                            context.Customers.Remove(selectedItem);
                            context.SaveChanges();
                            AllCustomerList.Remove(selectedItem);
                            CustomerList = new ObservableCollection<Customer>(AllCustomerList);
                            OnPropertyChanged(nameof(CustomerList));
                            OnPropertyChanged(nameof(AllCustomerList));
                            MessageBox.Show("Delete Successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void Add(object obj)
        {
            Validator validator = new Validator();
            if (textBoxItem.FullName.IsNullOrEmpty() ||
                 textBoxItem.Gender.IsNullOrEmpty() ||
                 textBoxItem.Email.IsNullOrEmpty() ||
                 textBoxItem.PhoneNumber.IsNullOrEmpty() ||
                 textBoxItem.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (!validator.IsValidEmail(textBoxItem.Email))
            {
                MessageBox.Show("Invalid email format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!validator.IsValidPhone(textBoxItem.PhoneNumber))
            {
                MessageBox.Show("Phone number must be number and length from 10 to 11 digits", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (textBoxItem.Birthday > DateTime.Now)
            {
                MessageBox.Show("Birthday cannot be in the future", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (validator.IsEmailExistsCustomer(textBoxItem.Email))
            {
                MessageBox.Show("Email already exists", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!validator.IsValidPassword(textBoxItem.Password))
            {
                MessageBox.Show("Password must be at least 8 characters long, contain at least one uppercase letter and one special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                var item = new Customer
                {
                    Password = PasswordBoxHelper.GetMD5(textBoxItem.Password),
                    Email = textBoxItem.Email,
                    PhoneNumber = textBoxItem.PhoneNumber,
                    FullName = textBoxItem.FullName,
                    Birthday = textBoxItem.Birthday,
                    Gender = textBoxItem.Gender,
                    CreatedDate = DateTime.Now
                };
                using (var context = new FstoreContext())
                {
                    context.Customers.Add(item);
                    context.SaveChanges();
                }
                OnCustomerAdded?.Invoke();
                textBoxItem = new Customer();
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
            Validator validator = new Validator();
            if (textBoxItem.FullName.IsNullOrEmpty() ||
                 textBoxItem.Gender.IsNullOrEmpty() ||
                 textBoxItem.Email.IsNullOrEmpty() ||
                 textBoxItem.PhoneNumber.IsNullOrEmpty() ||
                 textBoxItem.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (!validator.IsValidEmail(textBoxItem.Email))
            {
                MessageBox.Show("Invalid email format", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else if (!validator.IsValidPhone(textBoxItem.PhoneNumber))
            {
                MessageBox.Show("Phone number must be number and length from 10 to 11 digits", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            else if (textBoxItem.Birthday > DateTime.Now)
            {
                MessageBox.Show("Birthday cannot be in the future", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            else
            {
                if (!textBoxItem.Password.Equals(selectedItem.Password))
                {
                    if (validator.IsValidPassword(textBoxItem.Password))
                    {
                        TextBoxItem.Password = PasswordBoxHelper.GetMD5(textBoxItem.Password);
                    }
                    else
                    {
                        MessageBox.Show("Password must be at least 8 characters long, contain at least one uppercase letter and one special character!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                if (!selectedItem.Email.Equals(textBoxItem.Email))
                {
                    if (validator.IsEmailExistsCustomer(textBoxItem.Email))
                    {
                        MessageBox.Show("This email has been using!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                }

                using (var context = new FstoreContext())
                {
                    context.Customers.Update(textBoxItem);
                    context.SaveChanges();
                }

                OnCustomerAdded?.Invoke();
                textBoxItem = new Customer();
                OnPropertyChanged(nameof(TextBoxItem));

                Application.Current.Windows[2]?.Close();
                Application.Current.Windows[0].Opacity = 1;
                Application.Current.Windows[0].Focus();
                Application.Current.Windows[0].IsHitTestVisible = true;

                MessageBox.Show("Update Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
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
                var list = from item in AllCustomerList
                           where item.FullName.ToLower().Contains(searchBoxItem.ToLower())
                           select item;
                if (list.Any())
                {
                    CustomerList = new ObservableCollection<Customer>(list);
                }
                else
                {
                    MessageBox.Show("There is no customer", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (searchBoxItem.Equals(""))
            {
                CustomerList = new ObservableCollection<Customer>(AllCustomerList);
                OnPropertyChanged(nameof(CustomerList));
            }
        }

        public bool IsValidPassword(string password)
        {
            if (password.Length < 8 ||
                   !password.Any(char.IsUpper) ||
                   !password.Any(char.IsLower) ||
                   !password.Any(char.IsDigit) ||
                   !password.Any(ch => !char.IsLetterOrDigit(ch)))
            {
                MessageBox.Show("Password must be contain at least 8 character, uppercase and special characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        public bool IsValidPhoneNumber(string phoneNumber)
        {
            if (!phoneNumber.All(char.IsDigit))
            {
                MessageBox.Show("Phone number can only contain digits.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (phoneNumber.Length > 15)
            {
                MessageBox.Show("Phone number cannot exceed 15 characters.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
            return true;
        }
        public bool ValidateDate(DateTime? inputDate)
        {
            DateTime today = DateTime.Today;

            if (inputDate >= today)
            {
                MessageBox.Show("The selected date cannot be today or a future date.",
                                 "Validation Error",
                                 MessageBoxButton.OK,
                                 MessageBoxImage.Error);
                return false;
            }

            return true;
        }
    }
}
