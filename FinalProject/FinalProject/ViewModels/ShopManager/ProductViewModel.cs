﻿using System;
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
        public Dictionary<int, bool> SelectedProducts { get; set; } = new Dictionary<int, bool>();
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ICommand ExportCommand { get; }
        public ICommand DeleteSelectedCommand { get; }
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
            DeleteSelectedCommand = new RelayCommand(DeleteSelected);
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

                products = new ObservableCollection<Product>(list);
                allproducts = new ObservableCollection<Product>(products);
            }

            foreach (var product in products)
            {
                if (!SelectedProducts.ContainsKey(product.ProductId))
                    SelectedProducts[product.ProductId] = false;
            }

            OnPropertyChanged(nameof(products));
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
            // Lấy danh sách các sản phẩm được tick checkbox
            var multiSelected = SelectedProducts
                .Where(kvp => kvp.Value)
                .Select(kvp => products.FirstOrDefault(p => p.ProductId == kvp.Key))
                .Where(p => p != null)
                .ToList();

            // XÓA HÀNG LOẠT
            if (multiSelected.Any())
            {
                var confirmMsg = $"Are you sure you want to delete {multiSelected.Count} selected product(s)?";
                var result = MessageBox.Show(confirmMsg, "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (result != MessageBoxResult.Yes) return;

                using (var context = new FstoreContext())
                {
                    foreach (var product in multiSelected)
                    {
                        var dbProduct = context.Products
                            .Include(p => p.OrderDetails)
                            .Include(p => p.ImportOrderDetails)
                            .FirstOrDefault(p => p.ProductId == product.ProductId);

                        if (dbProduct != null)
                        {
                            if (dbProduct.OrderDetails.Any() || dbProduct.ImportOrderDetails.Any())
                            {
                                MessageBox.Show($"Cannot delete {product.FullName} because it has related order details.",
                                                "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                                continue;
                            }

                            context.Products.Remove(dbProduct);
                            products.Remove(product);
                            SelectedProducts[product.ProductId] = false;
                        }
                    }
                    context.SaveChanges();
                }

                allproducts = new ObservableCollection<Product>(products);
                OnPropertyChanged(nameof(products));
                OnPropertyChanged(nameof(allproducts));

                MessageBox.Show($"{multiSelected.Count} product(s) deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            // XÓA ĐƠN LẺ
            if (selectItem != null)
            {
                var confirm = MessageBox.Show($"Are you sure you want to delete {selectItem.FullName}?",
                                              "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (confirm != MessageBoxResult.Yes) return;

                using (var context = new FstoreContext())
                {
                    var dbProduct = context.Products
                        .Include(p => p.OrderDetails)
                        .Include(p => p.ImportOrderDetails)
                        .FirstOrDefault(p => p.ProductId == selectItem.ProductId);

                    if (dbProduct != null)
                    {
                        if (dbProduct.OrderDetails.Any() || dbProduct.ImportOrderDetails.Any())
                        {
                            MessageBox.Show("This product cannot be deleted because it has related order details.",
                                            "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }

                        context.Products.Remove(dbProduct);
                        context.SaveChanges();

                        products.Remove(selectItem);
                        allproducts = new ObservableCollection<Product>(products);
                        OnPropertyChanged(nameof(products));
                        OnPropertyChanged(nameof(allproducts));

                        MessageBox.Show($"{selectItem.FullName} deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Product not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Reset form nhập
                textboxItem = new Product { Brand = new Brand(), Category = new Category() };
                OnPropertyChanged(nameof(textboxItem));
                return;
            }

            // KHÔNG CHỌN GÌ HẾT
            MessageBox.Show("Please select a product to delete, or tick checkboxes for batch deletion.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    Filter = "Excel files (*.xlsx)|*.xlsx",
                    FileName = "products.xlsx",
                    Title = "Save Exported Data"
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    var workbook = new ClosedXML.Excel.XLWorkbook();
                    var worksheet = workbook.Worksheets.Add("Products");

                    var headerRange = worksheet.Range(1, 1, 1, 9);
                    headerRange.Style.Font.Bold = true;
                    headerRange.Style.Alignment.Horizontal = ClosedXML.Excel.XLAlignmentHorizontalValues.Center;
                    worksheet.Columns().AdjustToContents();

                    // Header row
                    worksheet.Cell(1, 1).Value = "Product ID";
                    worksheet.Cell(1, 2).Value = "Brand";
                    worksheet.Cell(1, 3).Value = "Category";
                    worksheet.Cell(1, 4).Value = "Model";
                    worksheet.Cell(1, 5).Value = "Full Name";
                    worksheet.Cell(1, 6).Value = "Description";
                    worksheet.Cell(1, 7).Value = "Price";
                    worksheet.Cell(1, 8).Value = "Stock";
                    worksheet.Cell(1, 9).Value = "Disable";

                    // Data rows
                    for (int i = 0; i < allproducts.Count; i++)
                    {
                        var p = allproducts[i];
                        worksheet.Cell(i + 2, 1).Value = p.ProductId;
                        worksheet.Cell(i + 2, 2).Value = p.Brand?.Name ?? "";
                        worksheet.Cell(i + 2, 3).Value = p.Category?.Name ?? "";
                        worksheet.Cell(i + 2, 4).Value = p.Model;
                        worksheet.Cell(i + 2, 5).Value = p.FullName;
                        worksheet.Cell(i + 2, 6).Value = p.Description;
                        worksheet.Cell(i + 2, 7).Value = p.Price;
                        worksheet.Cell(i + 2, 8).Value = p.Stock;
                        worksheet.Cell(i + 2, 9).Value = p.IsDeleted == true ? "Yes" : "No";
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

        private void DeleteSelected(object obj)
        {
            var selectedProducts = products.Where(p => SelectedProducts.ContainsKey(p.ProductId) && SelectedProducts[p.ProductId]).ToList();

            if (!selectedProducts.Any())
            {
                MessageBox.Show("No products selected.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            using (var context = new FstoreContext())
            {
                foreach (var product in selectedProducts)
                {
                    var dbProduct = context.Products.Find(product.ProductId);
                    if (dbProduct != null)
                    {
                        context.Products.Remove(dbProduct);
                    }
                }
                context.SaveChanges();
            }

            foreach (var product in selectedProducts)
            {
                products.Remove(product);
                SelectedProducts.Remove(product.ProductId);
            }

            allproducts = new ObservableCollection<Product>(products);
            OnPropertyChanged(nameof(products));

            MessageBox.Show($"{selectedProducts.Count} product(s) deleted successfully.", "Deleted", MessageBoxButton.OK, MessageBoxImage.Information);

            foreach (var product in SelectedProducts.Keys.ToList())
            {
                SelectedProducts[product] = false;
            }
        }

    }
}
