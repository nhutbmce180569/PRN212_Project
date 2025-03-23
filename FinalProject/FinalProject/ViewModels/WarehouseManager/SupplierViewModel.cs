using FinalProject.Models;
using FinalProject.Views.WarehouseManager.Supplier;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using WPFLab.Helper;
using WPFLab.ViewModels;

namespace FinalProject.ViewModels.WarehouseManager
{
    internal class SupplierViewModel : BaseViewModel
    {
        public event Action OnSupplierAdded;
        public ObservableCollection<Supplier> _supplierList;
        public ObservableCollection<Supplier> SupplierList
        {
            get => _supplierList;
            set
            {
                _supplierList = value;
                OnPropertyChanged(nameof(SupplierList)); // Kích hoạt UI update
            }
        }
        public ObservableCollection<Supplier> AllSupplierList { set; get; }
        public ICommand OpenAddPopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public List<bool> StatusList { get; set; } = new List<bool> { true, false };
        public SupplierViewModel()
        {
            Load();
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
        }

        public SupplierViewModel(Supplier supplier)
        {
            SelectedItem = supplier;
            Load();
            OpenAddPopupCommand = new RelayCommand(OpenPopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Load()
        {
            using var context = new FstoreContext();
            var list = context.Suppliers.ToList();
            AllSupplierList = new ObservableCollection<Supplier>(list);
            SupplierList = new ObservableCollection<Supplier>(AllSupplierList);
        }

        private Supplier _textBoxItem = new Supplier();
        public Supplier TextBoxItem
        {
            get { return _textBoxItem; }
            set
            {
                _textBoxItem = value;
                OnPropertyChanged(nameof(TextBoxItem));
            }
        }

        private Supplier _selectedItem;
        public Supplier SelectedItem
        {
            get { return _selectedItem; }
            set
            {
                _selectedItem = value;
                OnPropertyChanged(nameof(_selectedItem));
                if (_selectedItem != null)
                {
                    _textBoxItem = JsonConvert.DeserializeObject<Supplier>(JsonConvert.SerializeObject(_selectedItem));
                    OnPropertyChanged(nameof(TextBoxItem));
                }
            }
        }

        private void OpenPopup(object obj)
        {
            var action = new SupplierViewModel();
            action.OnSupplierAdded += Load;
            var popup = new AddSupplier
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
            if (_selectedItem != null)
            {
                var action = new SupplierViewModel(_selectedItem);
                action.OnSupplierAdded += Load;
                var popup = new UpdateSupplier(_selectedItem)
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
                MessageBox.Show("Please select supplier to update", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }

        }

        private void Add(object obj)
        {
            if (_textBoxItem.TaxId.IsNullOrEmpty() ||
                _textBoxItem.Name.IsNullOrEmpty() ||
                _textBoxItem.Address.IsNullOrEmpty() ||
                _textBoxItem.Email.IsNullOrEmpty() ||
                _textBoxItem.PhoneNumber.IsNullOrEmpty())
            {
                Debug.WriteLine(_textBoxItem.TaxId);
                Debug.WriteLine(_textBoxItem.Name);
                Debug.WriteLine(_textBoxItem.Address);
                Debug.WriteLine(_textBoxItem.Email);
                Debug.WriteLine(_textBoxItem.PhoneNumber);
                MessageBox.Show("Input enough information", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!IsValidPhone(TextBoxItem.PhoneNumber))
                {
                    MessageBox.Show("Invalid phone number format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!IsValidEmail(TextBoxItem.Email))
                {
                    MessageBox.Show("Invalid email format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (TextBoxItem.Name.Length > 255)
                {
                    MessageBox.Show("Invalid name format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    try
                    {
                        var item = new Supplier
                        {
                            TaxId = _textBoxItem.TaxId,
                            Name = _textBoxItem.Name,
                            Address = _textBoxItem.Address,
                            Email = _textBoxItem.Email,
                            PhoneNumber = _textBoxItem.PhoneNumber,
                            CreatedDate = DateTime.Now
                        };

                        Debug.WriteLine(item.Name + item == null);

                        using var context = new FstoreContext();
                        context.Suppliers.Add(item);
                        context.SaveChanges();

                        AllSupplierList.Add(item);
                        SupplierList.Add(item);

                        OnSupplierAdded?.Invoke();
                        _textBoxItem = new Supplier();
                        OnPropertyChanged(nameof(TextBoxItem));
                        Application.Current.Windows[2]?.Close();
                        Application.Current.Windows[0].Opacity = 1;
                        Application.Current.Windows[0].Focus();
                        Application.Current.Windows[0].IsHitTestVisible = true;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi lưu vào database: {ex.Message}\n{ex.InnerException?.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }
        private void Update(object obj)
        {
            if (_textBoxItem.TaxId.IsNullOrEmpty() ||
                _textBoxItem.Name.IsNullOrEmpty() ||
                _textBoxItem.Address.IsNullOrEmpty() ||
                _textBoxItem.Email.IsNullOrEmpty() ||
                _textBoxItem.PhoneNumber.IsNullOrEmpty())
            {
                MessageBox.Show("Input enough information", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                if (!IsValidPhone(_textBoxItem.PhoneNumber))
                {
                    MessageBox.Show("Invalid phone number format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (!IsValidEmail(_textBoxItem.Email))
                {
                    MessageBox.Show("Invalid email format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else if (_textBoxItem.Name.Length > 255)
                {
                    MessageBox.Show("Invalid name format", "Erorr", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                else
                {
                    using (var context = new FstoreContext())
                    {
                        context.Suppliers.Update(_textBoxItem);
                        context.SaveChanges();
                    }
                    OnSupplierAdded?.Invoke();
                    _textBoxItem = new Supplier();
                    OnPropertyChanged(nameof(TextBoxItem));
                    Application.Current.Windows[2]?.Close();
                    Application.Current.Windows[0].Opacity = 1;
                    Application.Current.Windows[0].Focus();
                    Application.Current.Windows[0].IsHitTestVisible = true;
                }
            }
        }
        private void Delete(object obj)
        {
            if (_selectedItem == null)
            {
                MessageBox.Show("Please select supplier to delete", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                if (MessageBox.Show("Are you sure to delete this supplier?", "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.Yes)
                {
                    try
                    {
                        using var context = new FstoreContext();
                        context.Suppliers.Remove(_selectedItem);
                        context.SaveChanges();
                        SupplierList.Remove(_selectedItem);
                        AllSupplierList.Remove(_selectedItem);
                        MessageBox.Show("Delete Successfully", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    catch (Exception e)
                    {
                        MessageBox.Show("This supplier is providing products\n Cannot delete", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
        }
        private String _searchBoxItem;
        public String SearchBoxItem
        {
            get { return _searchBoxItem; }
            set
            {
                _searchBoxItem = value;
                OnPropertyChanged(nameof(SearchBoxItem));
            }
        }
        private void Search(object obj)
        {
            if (!_searchBoxItem.Equals(""))
            {
                var list = from item in AllSupplierList
                           where item.Name.ToLower().Contains(_searchBoxItem.ToLower())
                           select item;
                if (list.Any())
                {
                    SupplierList = new ObservableCollection<Supplier>(list);
                }
                else
                {
                    MessageBox.Show("There is no supplier", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else if (_searchBoxItem.Equals(""))
            {
                SupplierList = new ObservableCollection<Supplier>(AllSupplierList);
                OnPropertyChanged(nameof(SupplierList));
            }
        }
        public bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return false;

            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
            return Regex.IsMatch(email, pattern);
        }
        public bool IsValidPhone(string input)
        {
            return !string.IsNullOrEmpty(input) && input.Length == 10 && input.All(char.IsDigit);
        }

    }
}
