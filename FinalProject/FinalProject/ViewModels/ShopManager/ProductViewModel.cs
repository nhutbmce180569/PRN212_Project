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

        public ProductViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
            SearchCommand = new RelayCommand(Search);
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
            }
            allproducts = new ObservableCollection<Product>(products);
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

                if (_selectItem != null)
                {
                    // Sao chép dữ liệu sang textboxItem
                    textboxItem = new Product
                    {
                        ProductId = _selectItem.ProductId,
                        Brand = _selectItem.Brand != null ? new Brand { BrandId = _selectItem.Brand.BrandId, Name = _selectItem.Brand.Name } : new Brand(),
                        Category = _selectItem.Category != null ? new Category { CategoryId = _selectItem.Category.CategoryId, Name = _selectItem.Category.Name } : new Category(),
                        Model = _selectItem.Model,
                        FullName = _selectItem.FullName,
                        Description = _selectItem.Description,
                        IsDeleted = _selectItem.IsDeleted,
                        Price = _selectItem.Price,
                        Stock = _selectItem.Stock
                    };

                    OnPropertyChanged(nameof(textboxItem)); // Cập nhật giao diện
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


        private void Update(object obj)
        {
            if (_selectItem != null)
            {
                using (var context = new FstoreContext())
                {
                    context.Products.Update(_textboxItem);
                    context.SaveChanges();
                }

                //cập nhật observablecollection
                int index = products.IndexOf(_selectItem);
                if (index >= 0)
                {
                    products[index] = _textboxItem;
                }

                //gán dữ liệu vào allstudents (observablecollection)
                allproducts = new ObservableCollection<Product>(products);

                textboxItem = new Product();
                OnPropertyChanged(nameof(textboxItem)); //phải có
            }
        }

        private void Add(object obj)
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

    }
}
