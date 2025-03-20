using FinalProject.Models;
using FinalProject.Views.WarehouseManager.ImportOrder;
using Microsoft.EntityFrameworkCore;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using System.Windows.Input;
using WPFLab.Helper;
using WPFLab.ViewModels;

namespace FinalProject.ViewModels.WarehouseManager
{
    class ProductViewModel : BaseViewModel
    {
        public event Action LoadInformation;
        public ObservableCollection<Product> allproducts { get; set; }
        public ObservableCollection<Product> products { get; set; }
        public ICommand SearchCommand { get; }
        public ICommand ImportCommand { get; }
        public ICommand OpenImportPopupCommand { get; }
        public ObservableCollection<Supplier> Suppliers { get; set; }
        public ProductViewModel()
        {
            Load();
            ImportCommand = new RelayCommand(ImportProduct);
            SearchCommand = new RelayCommand(Search);
            OpenImportPopupCommand = new RelayCommand(OpenImportPopup);
        }
        private void Load()
        {
            using var context = new FstoreContext();
            var list = context.Products
                .Include(p => p.Brand)
            .Include(p => p.Category)
            .ToList();

            Suppliers = new ObservableCollection<Supplier>(context.Suppliers.Where(s => s.IsActivate == true).ToList());
            allproducts = new ObservableCollection<Product>(list);
            products = new ObservableCollection<Product>(allproducts);
        }

        private Product _textboxItem;
        public Product TextboxItem
        {
            get { return _textboxItem; }
            set
            {
                _textboxItem = value;
                OnPropertyChanged(nameof(TextboxItem));
            }
        }

        private Product? _importedProduct;
        private Product _selectItem;
        public Product SelectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;


                // Lấy đầy đủ thông tin Brand nếu chưa có
                if (_selectItem != null)
                {
                    using var context = new FstoreContext();
                    if (_selectItem.Brand == null)
                    {
                        _selectItem = context.Products
                            .Include(p => p.Brand)
                            .FirstOrDefault(p => p.ProductId == _selectItem.ProductId);
                    }
                    if (_selectItem.Category == null)
                    {
                        _selectItem = context.Products
                            .Include(p => p.Category)
                            .FirstOrDefault(p => p.ProductId == _selectItem.ProductId);
                    }

                    // Sao chép dữ liệu vào textboxItem nếu có sản phẩm được chọn

                    _textboxItem = new Product
                    {
                        ProductId = _selectItem.ProductId,
                        Brand = _selectItem.Brand,
                        Category = _selectItem.Category,
                        Model = _selectItem.Model,
                        FullName = _selectItem.FullName,
                        Description = _selectItem.Description,
                        Price = _selectItem.Price,
                        Stock = _selectItem.Stock,
                        IsDeleted = _selectItem.IsDeleted
                    };
                    Debug.WriteLine(_selectItem.ProductId);
                    Debug.WriteLine(_selectItem.Brand);
                    Debug.WriteLine(_selectItem.Category);
                    Debug.WriteLine(_selectItem.Model);
                    Debug.WriteLine(_selectItem.FullName);
                    Debug.WriteLine(_selectItem.Description);

                }

                OnPropertyChanged(nameof(SelectItem));
            }
        }

        private string _searchText;
        public string SearchText
        {
            get { return _searchText; }
            set
            {
                _searchText = value;
                OnPropertyChanged(nameof(SearchText));
            }
        }
        private void Search(object obj)
        {
            if (string.IsNullOrWhiteSpace(SearchText))
            {
                // Nếu không có từ khóa tìm kiếm, hiển thị toàn bộ danh sách sản phẩm
                products = new ObservableCollection<Product>(allproducts);
            }
            else
            {
                using (var context = new FstoreContext())
                {
                    var searchQuery = SearchText.ToLower();

                    var filteredProducts = context.Products
                        .Include(p => p.Brand)
                        .Include(p => p.Category)
                        .Where(p =>
                            p.Model.ToLower().Contains(searchQuery) ||
                            p.FullName.ToLower().Contains(searchQuery) ||
                            (p.Brand != null && p.Brand.Name.ToLower().Contains(searchQuery)) ||
                            (p.Category != null && p.Category.Name.ToLower().Contains(searchQuery)))
                        .ToList();

                    products = new ObservableCollection<Product>(filteredProducts);
                }
            }

            OnPropertyChanged(nameof(products));
        }

        private int _importedQuantity;
        public int ImportedQuantity
        {
            get { return _importedQuantity; }
            set
            {
                _importedQuantity = value;
                OnPropertyChanged(nameof(ImportedQuantity));
            }
        }

        private int _importedPrice;
        public int ImportedPrice
        {
            get { return _importedPrice; }
            set
            {
                _importedPrice = value;
                OnPropertyChanged(nameof(ImportedPrice));
            }
        }
        private Supplier _selectedSupplier;
        public Supplier SelectedSupplier
        {
            get { return _selectedSupplier; }
            set
            {
                _selectedSupplier = value;
                OnPropertyChanged(nameof(SelectedSupplier));
            }
        }

        private void ImportProduct(object obj)
        {
            if (ImportedQuantity < 1)
            {
                MessageBox.Show("Quantity must be greater than 0", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (TextboxItem == null)
            {
                MessageBox.Show("Please select product you want to import quantity", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else if (ImportedPrice < 1000)
            {
                MessageBox.Show("Price cannot be less than 1000", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                using (var context = new FstoreContext())
                {
                    Product? importedProduct = context.Products.Find(TextboxItem.ProductId);
                    if (SelectedSupplier == null)
                    {
                        MessageBox.Show("Please select Supplier", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else if (importedProduct != null)
                    {
                        importedProduct.Stock += ImportedQuantity;
                        context.Update(importedProduct);

                        // thu nghiem

                        ImportOrder iO = new ImportOrder() { TotalCost = ImportedPrice * ImportedQuantity, ImportDate = DateTime.Now, SupplierId = SelectedSupplier.SupplierId };
                        context.ImportOrders.Add(iO);
                        context.SaveChanges();

                        ImportOrderDetail iOD = new ImportOrderDetail() { Io = iO, ProductId = importedProduct.ProductId, Quantity = ImportedQuantity, ImportPrice = ImportedPrice };
                        context.ImportOrderDetails.Add(iOD);

                        // thu nghiem

                        context.SaveChanges();

                        LoadInformation();

                        OnPropertyChanged(nameof(products));
                        OnPropertyChanged(nameof(allproducts));

                        // Xóa TextboxItem sau khi import xong
                        _textboxItem = new Product();
                        OnPropertyChanged(nameof(TextboxItem));

                        // 🔥 Đóng popup an toàn hơn
                        Application.Current.Windows[2]?.Close();
                        Application.Current.Windows[0].Opacity = 1;
                        Application.Current.Windows[0].Focus();
                        Application.Current.Windows[0].IsHitTestVisible = true;
                    }
                }
            }
        }
        private void OpenImportPopup(object obj)
        {
            if (SelectItem == null)
            {
                MessageBox.Show("Please select product you want to import quantity", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            LoadInformation += Load;

            var popup = new ImportProduct();
            popup.DataContext = this;
            popup.ShowDialog();
        }
    }
}
