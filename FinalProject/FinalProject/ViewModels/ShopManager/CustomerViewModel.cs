using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using FinalProject.Models;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using WPFLab.Helper;
using FinalProject.Views.ShopManager.Customer;
using System.Windows;
using WPFLab.ViewModels;
using Newtonsoft.Json;
using Microsoft.IdentityModel.Tokens;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using FinalProject.Views.ShopManager;
using System.Diagnostics;
using System.Windows.Documents;
using System.Text.RegularExpressions;
using FinalProject.Helper;

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

        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public CustomerViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
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
            popup.Deactivated += (s, e) =>
            {
                Application.Current.Windows[0].IsHitTestVisible = false;
                if (popup.IsLoaded) //fdjskfjdskfds
                {

                    Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        if (!popup.IsActive) // gggfgfh
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
                var popup = new UpdateCustomer(selectedItem)
                {
                    DataContext = action,
                    Owner = Application.Current.MainWindow,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                popup.Deactivated += (s, e) =>
                {
                    if (popup.IsLoaded)
                    {
                        Application.Current.Windows[0].IsHitTestVisible = false;

                        Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                        {
                            if (!popup.IsActive) 
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
                            CustomerList.Remove(selectedItem);
                            AllCustomerList.Remove(selectedItem);
                            MessageBox.Show("Delete Successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                        }
                    }
                }
            }
        }

        private void Add(object obj)
        {
            if (textBoxItem.FullName.IsNullOrEmpty() ||
                textBoxItem.PhoneNumber.IsNullOrEmpty() ||
                textBoxItem.Email.IsNullOrEmpty() ||
                textBoxItem.Password.IsNullOrEmpty()||
                textBoxItem.Gender.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (IsValidEmail(textBoxItem.Email)) {
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
                else
                {
                    MessageBox.Show("Invalid email format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            }
        }

        private void Update(object obj)
        {
            if (textBoxItem.FullName.IsNullOrEmpty() ||
           textBoxItem.PhoneNumber.IsNullOrEmpty() ||
           textBoxItem.Email.IsNullOrEmpty() ||
           textBoxItem.Password.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                TextBoxItem.Password = PasswordBoxHelper.GetMD5(textBoxItem.Password);
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
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
    }
}
