using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;
using System.Windows.Input;
using System.Windows;
using WPFLab.Helper;
using WPFLab.ViewModels;
using Microsoft.EntityFrameworkCore;
using FinalProject.Views.ShopManager.Product;
using Newtonsoft.Json;
using System.IO;
using Microsoft.Win32;

namespace FinalProject.ViewModels.ShopManager
{
    class ProductViewModel : BaseViewModel
    {
        public ObservableCollection<Product> allproducts { get; set; }
        public ObservableCollection<Product> products { get; set; }
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand OpenCreatePopupCommand { get; }
        public ICommand OpenUpdatePopupCommand { get; }
        public ObservableCollection<Brand> Brands { get; set; }
        public ObservableCollection<Category> Categories { get; set; }

        public ProductViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
            ExportCommand = new RelayCommand(Export);
            OpenCreatePopupCommand = new RelayCommand(OpenCreatePopup);
            OpenUpdatePopupCommand = new RelayCommand(OpenUpdatePopup);

        }
        private void Load()
        {
            using (var context = new FstoreContext())
            {
                var list = context.Products
                    .Include(p => p.Brand)
                    .Include(p => p.Category)
                    .ToList();

                Brands = new ObservableCollection<Brand>(context.Brands.ToList());
                Categories = new ObservableCollection<Category>(context.Categories.ToList());

                products = new ObservableCollection<Product>(list);
                allproducts = new ObservableCollection<Product>(products);
            }

            OnPropertyChanged(nameof(Brands));
            OnPropertyChanged(nameof(Categories));
        }


        private Product _textboxItem;
        public Product textboxItem
        {
            get { return _textboxItem; }
            set
            {
                _textboxItem = value;
                OnPropertyChanged(nameof(textboxItem));
            }
        }

        private Product _selectItem;
        public Product selectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                OnPropertyChanged(nameof(selectItem));

                CanUpdate = _selectItem != null;

                if (_selectItem != null)
                {
                    // Tìm Brand và Category từ danh sách hiện tại
                    var selectedBrand = Brands?.FirstOrDefault(b => b.BrandId == _selectItem.Brand?.BrandId);
                    var selectedCategory = Categories?.FirstOrDefault(c => c.CategoryId == _selectItem.Category?.CategoryId);

                    textboxItem = new Product
                    {
                        ProductId = _selectItem.ProductId,
                        Brand = selectedBrand ?? _selectItem.Brand,  // Đảm bảo lấy đúng Brand trong danh sách
                        Category = selectedCategory ?? _selectItem.Category,  // Đảm bảo lấy đúng Category trong danh sách
                        Model = _selectItem.Model,
                        FullName = _selectItem.FullName,
                        Description = _selectItem.Description,
                        Price = _selectItem.Price,
                        Stock = _selectItem.Stock,
                        IsDeleted = _selectItem.IsDeleted
                    };

                    OnPropertyChanged(nameof(textboxItem));
                }
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
        private void Delete(object obj)
        {
            if (_selectItem == null)
            {
                MessageBox.Show("Please select a product to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_selectItem.FullName}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new FstoreContext())
                {
                    // Tìm lại sản phẩm trong database
                    var product = context.Products
                        .Include(p => p.OrderDetails)
                        .Include(p => p.ImportOrderDetails)
                        .FirstOrDefault(p => p.ProductId == _selectItem.ProductId);

                    if (product != null)
                    {
                        // Kiểm tra nếu sản phẩm có dữ liệu trong OrderDetails hoặc ImportOrderDetails
                        if (product.OrderDetails.Any() || product.ImportOrderDetails.Any())
                        {
                            MessageBox.Show("This product cannot be deleted because it has related order details.",
                                            "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        // Nếu không có dữ liệu liên quan, tiến hành xóa
                        context.Products.Remove(product);
                        context.SaveChanges();

                        // Cập nhật danh sách UI
                        products.Remove(_selectItem);
                        allproducts = new ObservableCollection<Product>(products);
                        OnPropertyChanged(nameof(products));
                        OnPropertyChanged(nameof(allproducts));
                    }
                    else
                    {
                        MessageBox.Show("Product not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Reset form nhập
                textboxItem = new Product { Brand = new Brand(), Category = new Category() };
                OnPropertyChanged(nameof(textboxItem));
            }
        }
        private bool _canUpdate;
        public bool CanUpdate
        {
            get { return _canUpdate; }
            set
            {
                _canUpdate = value;
                OnPropertyChanged(nameof(CanUpdate));
            }
        }
        private void Update(object obj)
        {
            try
            {
                if (textboxItem == null || textboxItem.ProductId == 0)
                {
                    MessageBox.Show("No product selected for update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (textboxItem == null || string.IsNullOrWhiteSpace(textboxItem.Brand?.Name) ||
                    string.IsNullOrWhiteSpace(textboxItem.Category?.Name) ||
                    string.IsNullOrWhiteSpace(textboxItem.Model) ||
                    string.IsNullOrWhiteSpace(textboxItem.FullName) ||
                    string.IsNullOrWhiteSpace(textboxItem.Description))
                {
                    MessageBox.Show("Input enough information", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (textboxItem.Price < 0)
                    {
                        MessageBox.Show("Product prices cannot be negative.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (textboxItem.FullName.Length > 255)
                    {
                        MessageBox.Show("Product names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    if (textboxItem.Model.Length > 50)
                    {
                        MessageBox.Show("Model cannot exceed 50 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                    using (var context = new FstoreContext())
                    {
                        var existingProduct = context.Products.Find(textboxItem.ProductId);
                        if (existingProduct != null)
                        {
                            existingProduct.BrandId = textboxItem.Brand?.BrandId;
                            existingProduct.CategoryId = textboxItem.Category?.CategoryId;
                            existingProduct.Model = textboxItem.Model;
                            existingProduct.FullName = textboxItem.FullName;
                            existingProduct.Description = textboxItem.Description;
                            existingProduct.Price = textboxItem.Price;
                            existingProduct.Stock = textboxItem.Stock;
                            existingProduct.IsDeleted = textboxItem.IsDeleted;

                            context.SaveChanges();
                        }
                    }

                    // Cập nhật danh sách sản phẩm trong UI
                    var index = products.IndexOf(selectItem);
                    if (index >= 0)
                    {
                        products[index] = textboxItem;
                    }

                    allproducts = new ObservableCollection<Product>(products);
                    OnPropertyChanged(nameof(products));
                    OnPropertyChanged(nameof(allproducts));
                    ClosePopup();
                    MessageBox.Show("Update Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Add(object obj)
        {
            try
            {
                if (textboxItem == null || string.IsNullOrWhiteSpace(textboxItem.Brand?.Name) ||
                    string.IsNullOrWhiteSpace(textboxItem.Category?.Name) ||
                    string.IsNullOrWhiteSpace(textboxItem.Model) ||
                    string.IsNullOrWhiteSpace(textboxItem.FullName) ||
                    string.IsNullOrWhiteSpace(textboxItem.Description))
                {
                    MessageBox.Show("Input enough information", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    using (var context = new FstoreContext())
                    {
                        // Kiểm tra xem brand có tồn tại chưa, nếu chưa thì tạo mới
                        var brand = context.Brands.FirstOrDefault(b => b.Name == textboxItem.Brand.Name);
                        if (brand == null)
                        {
                            brand = new Brand { Name = textboxItem.Brand.Name };
                            context.Brands.Add(brand);
                            context.SaveChanges(); // Lưu để lấy BrandId mới
                        }

                        // Kiểm tra xem category có tồn tại chưa, nếu chưa thì tạo mới
                        var category = context.Categories.FirstOrDefault(c => c.Name == textboxItem.Category.Name);
                        if (category == null)
                        {
                            category = new Category { Name = textboxItem.Category.Name };
                            context.Categories.Add(category);
                            context.SaveChanges(); // Lưu để lấy CategoryId mới
                        }
                        if (textboxItem.Price < 0)
                        {
                            MessageBox.Show("Product prices cannot be negative.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (textboxItem.FullName.Length > 255)
                        {
                            MessageBox.Show("Product names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        if (textboxItem.Model.Length > 50)
                        {
                            MessageBox.Show("Model cannot exceed 50 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Tạo sản phẩm mới
                        var newitem = new Product
                        {
                            BrandId = brand.BrandId, // Lấy BrandId từ brand vừa tạo hoặc tìm thấy
                            CategoryId = category.CategoryId, // Lấy CategoryId từ category vừa tạo hoặc tìm thấy
                            Model = textboxItem.Model,
                            FullName = textboxItem.FullName,
                            Description = textboxItem.Description,
                            IsDeleted = textboxItem.IsDeleted,
                            Price = textboxItem.Price,
                            Stock = textboxItem.Stock
                        };

                        // Thêm sản phẩm vào CSDL
                        context.Products.Add(newitem);
                        context.SaveChanges();

                        // Thêm vào danh sách observablecollection trên UI
                        products.Add(newitem);
                        allproducts = new ObservableCollection<Product>(products);

                        // Reset textbox
                        textboxItem = new Product();
                        OnPropertyChanged(nameof(textboxItem));
                        ClosePopup();
                        MessageBox.Show("Create Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
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
                            p.FullName.ToLower().Contains(searchQuery)
                            // || p.Model.ToLower().Contains(searchQuery) ||
                            //(p.Brand != null && p.Brand.Name.ToLower().Contains(searchQuery)) ||
                            //(p.Category != null && p.Category.Name.ToLower().Contains(searchQuery))
                            )
                        .ToList();

                    products = new ObservableCollection<Product>(filteredProducts);
                }
            }

            OnPropertyChanged(nameof(products));
        }
        private void OpenCreatePopup(object obj)
        {
            textboxItem = new Product
            {
                Brand = new Brand(),
                Category = new Category(),
                Model = "",
                FullName = "",
                Description = "",
                Price = 0,
                Stock = 0,
                IsDeleted = false
            };

            var popup = new AddProduct
            {
                DataContext = this,
                Owner = Application.Current.MainWindow, // Đặt Owner để đảm bảo nó thuộc về MainWindow
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            Application.Current.MainWindow.Opacity = 0.7; // Làm mờ MainWindow
            popup.ShowDialog();
            Application.Current.MainWindow.Opacity = 1; // Khôi phục lại MainWindow khi đóng popup
        }
        private void OpenUpdatePopup(object obj)
        {
            if (!CanUpdate)
            {
                MessageBox.Show("Please select a product to update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            var popup = new UpdateProduct
            {
                DataContext = this,
                Owner = Application.Current.MainWindow,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            Application.Current.MainWindow.Opacity = 0.7;
            popup.ShowDialog();
            Application.Current.MainWindow.Opacity = 1;
        }
        private void ClosePopup()
        {
            foreach (Window window in Application.Current.Windows)
            {
                if (window is AddProduct || window is UpdateProduct)
                {
                    window.Close();
                    break;
                }
            }
        }


        private void Export(object obj)
        {
            try
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    Filter = "JSON files (*.json)|*.json|All files (*.*)|*.*",
                    FileName = "products.json",
                    Title = "Save Exported Data"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    // Convert danh sách sản phẩm thành danh sách đơn giản để export
                    var exportList = allproducts.Select(p => new
                    {
                        p.ProductId,
                        Brand = p.Brand?.Name,
                        Category = p.Category?.Name,
                        p.Model,
                        p.FullName,
                        p.Description,
                        p.Price,
                        p.Stock,
                        p.IsDeleted
                    }).ToList();

                    // Serialize thành JSON
                    string json = JsonConvert.SerializeObject(exportList, Formatting.Indented);

                    // Ghi vào file
                    File.WriteAllText(saveFileDialog.FileName, json);

                    MessageBox.Show("Export successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error exporting data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
    }
}
