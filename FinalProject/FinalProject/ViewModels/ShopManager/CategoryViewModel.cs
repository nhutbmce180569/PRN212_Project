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

namespace FinalProject.ViewModels.ShopManager
{
    class CategoryViewModel : BaseViewModel
    {
        public ICommand AddCommand { get; }
        public ICommand UpdateCommand { get; }
        public ICommand DeleteCommand { get; }
        public ICommand SearchCommand { get; }
        public ObservableCollection<Category> categories { get; set; }
        public ObservableCollection<Category> allcategories { get; set; }

        public CategoryViewModel()
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
                var list = context.Categories.ToList();

                categories = new ObservableCollection<Category>(context.Categories.ToList());
                allcategories = new ObservableCollection<Category>(categories);
            }

            OnPropertyChanged(nameof(categories));
            var productViewModel = new ProductViewModel();
            productViewModel.RefreshData(); // Làm mới dữ liệu
        }

        private Category _textboxItem;
        public Category textboxItem
        {
            get { return _textboxItem; }
            set
            {
                _textboxItem = value;
                OnPropertyChanged(nameof(textboxItem));
            }
        }

        private Category _selectItem;
        public Category selectItem
        {
            get { return _selectItem; }
            set
            {
                _selectItem = value;
                OnPropertyChanged(nameof(selectItem));

                CanUpdate = _selectItem != null;

                if (_selectItem != null)
                {
                    textboxItem = new Category
                    {
                        CategoryId = _selectItem.CategoryId,
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
                MessageBox.Show("Please select a category to delete.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var result = MessageBox.Show($"Are you sure you want to delete {_selectItem.Name}?",
                                         "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes)
            {
                using (var context = new FstoreContext())
                {
                    // Kiểm tra xem có sản phẩm nào sử dụng thương hiệu này không
                    var productWithCategory = context.Products
                                                 .FirstOrDefault(p => p.CategoryId == _selectItem.CategoryId);

                    if (productWithCategory != null)
                    {
                        // Nếu có sản phẩm sử dụng thương hiệu này, không cho phép xóa
                        MessageBox.Show("Cannot delete this category because it is used by one or more products.",
                                         "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    // Nếu không có sản phẩm nào sử dụng, tiến hành xóa
                    var category = context.Categories
                                       .FirstOrDefault(b => b.CategoryId == _selectItem.CategoryId);
                    if (category != null)
                    {
                        context.Categories.Remove(category);
                        context.SaveChanges();

                        // Cập nhật danh sách UI
                        categories.Remove(_selectItem);
                        allcategories = new ObservableCollection<Category>(categories);
                        OnPropertyChanged(nameof(categories));
                        OnPropertyChanged(nameof(allcategories));
                        var productViewModel = new ProductViewModel();
                        productViewModel.RefreshData(); // Làm mới dữ liệu

                        MessageBox.Show("Delete successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                    else
                    {
                        MessageBox.Show("Brand not found in database.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Reset form nhập
                textboxItem = new Category();
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
                if (textboxItem == null || textboxItem.CategoryId == 0)
                {
                    MessageBox.Show("No category selected for update.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                        MessageBox.Show("Category names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    using (var context = new FstoreContext())
                    {
                        var existingCategory = context.Categories.Find(textboxItem.CategoryId);
                        if (existingCategory != null)
                        {
                            existingCategory.Name = textboxItem.Name;
                            context.SaveChanges();
                        }
                    }

                    // Cập nhật danh sách sản phẩm trong UI
                    var index = categories.IndexOf(selectItem);
                    if (index >= 0)
                    {
                        categories[index] = textboxItem;
                    }

                    allcategories = new ObservableCollection<Category>(categories);
                    OnPropertyChanged(nameof(categories));
                    OnPropertyChanged(nameof(allcategories));
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
                            MessageBox.Show("Category names cannot exceed 255 characters.", "Warning", MessageBoxButton.OK, MessageBoxImage.Error);
                            return;
                        }
                        // Tạo sản phẩm mới
                        var newitem = new Category
                        {
                            Name = textboxItem.Name
                        };

                        // Thêm sản phẩm vào CSDL
                        context.Categories.Add(newitem);
                        context.SaveChanges();

                        // Thêm vào danh sách observablecollection trên UI
                        categories.Add(newitem);
                        allcategories = new ObservableCollection<Category>(categories);

                        // Reset textbox
                        textboxItem = new Category();
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
