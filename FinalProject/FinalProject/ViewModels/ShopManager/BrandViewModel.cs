using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FinalProject.Models;
using FinalProject.Views.ShopManager.Product;
using Microsoft.Win32;
using System.Windows.Input;
using System.Windows;
using WPFLab.Helper;
using WPFLab.ViewModels;
using System.Windows.Navigation;

namespace FinalProject.ViewModels.ShopManager
{
    class BrandViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ObservableCollection<Brand> brands { get; set; }
        public ObservableCollection<Brand> allbrands { get; set; }

        public BrandViewModel()
        {
            Load();
            AddCommand = new RelayCommand(Add);
            UpdateCommand = new RelayCommand(Update);
            DeleteCommand = new RelayCommand(Delete);
        }

        private void Load()
        {
            using (var context = new FstoreContext())
            {
                var list = context.Brands.ToList();

                brands = new ObservableCollection<Brand>(context.Brands.ToList());
                allbrands = new ObservableCollection<Brand>(brands);
            }

            OnPropertyChanged(nameof(brands));
        }

        private Brand _textboxItem;
        public Brand textboxItem
        {
            get { return _textboxItem; }
            set
            {
                _textboxItem = value;
                OnPropertyChanged(nameof(textboxItem));
            }
        }

        private Brand _selectItem;
        public Brand selectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                OnPropertyChanged(nameof(selectItem));

                CanUpdate = _selectItem != null;

                if (_selectItem != null)
                {
                    textboxItem = new Brand
                    {
                        BrandId = _selectItem.BrandId,
                        Name = _selectItem.Name,
                    };

                    OnPropertyChanged(nameof(textboxItem));
                }
            }
        }

        private void Delete(object obj)
        {
            if (_selectItem == null)
            {
                MessageBox.Show("Please select a brand to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_selectItem.Name}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new FstoreContext())
                {
                    // Kiểm tra xem có sản phẩm nào sử dụng thương hiệu này không
                    var productWithBrand = context.Products
                                                 .FirstOrDefault(p => p.BrandId == _selectItem.BrandId);

                    if (productWithBrand != null)
                    {
                        // Nếu có sản phẩm sử dụng thương hiệu này, không cho phép xóa
                        MessageBox.Show("Cannot delete this brand because it is used by one or more products.",
                                         "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Nếu không có sản phẩm nào sử dụng, tiến hành xóa
                    var brand = context.Brands
                                       .FirstOrDefault(b => b.BrandId == _selectItem.BrandId);
                    if (brand != null)
                    {
                        context.Brands.Remove(brand);
                        context.SaveChanges();

                        // Cập nhật danh sách UI
                        brands.Remove(_selectItem);
                        allbrands = new ObservableCollection<Brand>(brands);
                        OnPropertyChanged(nameof(brands));
                        OnPropertyChanged(nameof(allbrands));

                        MessageBox.Show("Delete successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Brand not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Reset form nhập
                textboxItem = new Brand();
                OnPropertyChanged(nameof(textboxItem));
                var productViewModel = new ProductViewModel();
                productViewModel.RefreshData(); // Làm mới dữ liệu

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
                if (textboxItem == null || textboxItem.BrandId == 0)
                {
                    MessageBox.Show("No brand selected for update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                if (textboxItem == null ||
                    string.IsNullOrWhiteSpace(textboxItem.Name))
                {
                    MessageBox.Show("Input enough information", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    if (textboxItem.Name.Length > 255)
                    {
                        MessageBox.Show("Product names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    using (var context = new FstoreContext())
                    {
                        var existingProduct = context.Brands.Find(textboxItem.BrandId);
                        if (existingProduct != null)
                        {
                            existingProduct.Name = textboxItem.Name;
                            context.SaveChanges();
                        }
                    }

                    // Cập nhật danh sách sản phẩm trong UI
                    var index = brands.IndexOf(selectItem);
                    if (index >= 0)
                    {
                        brands[index] = textboxItem;
                    }

                    allbrands = new ObservableCollection<Brand>(brands);
                    OnPropertyChanged(nameof(brands));
                    OnPropertyChanged(nameof(allbrands));
                    var productViewModel = new ProductViewModel();
                    productViewModel.RefreshData(); // Làm mới dữ liệu
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
                if (textboxItem == null ||
                    string.IsNullOrWhiteSpace(textboxItem.Name))
                {
                    MessageBox.Show("Input enough information", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                else
                {
                    using (var context = new FstoreContext())
                    {

                        if (textboxItem.Name.Length > 255)
                        {
                            MessageBox.Show("Product names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Tạo sản phẩm mới
                        var newitem = new Brand
                        {
                            Name = textboxItem.Name
                        };

                        // Thêm sản phẩm vào CSDL
                        context.Brands.Add(newitem);
                        context.SaveChanges();

                        // Thêm vào danh sách observablecollection trên UI
                        brands.Add(newitem);
                        allbrands = new ObservableCollection<Brand>(brands);

                        // Reset textbox
                        textboxItem = new Brand();
                        OnPropertyChanged(nameof(textboxItem));
                        var productViewModel = new ProductViewModel();
                        productViewModel.RefreshData(); // Làm mới dữ liệu
                        MessageBox.Show("Create Successful", "Notification", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        } 
    }
}
