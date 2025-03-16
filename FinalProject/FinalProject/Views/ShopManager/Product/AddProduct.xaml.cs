using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FinalProject.Models;
using FinalProject.ViewModels.ShopManager;

namespace FinalProject.Views.ShopManager.Product
{
    /// <summary>
    /// Interaction logic for AddProduct.xaml
    /// </summary>
    public partial class AddProduct : Window
    {
        public AddProduct()
        {
            InitializeComponent();
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void AddNewBrand_Click(object sender, RoutedEventArgs e)
        {
            string brandName = Microsoft.VisualBasic.Interaction.InputBox("Enter new Brand Name:", "New Brand");
            if (!string.IsNullOrWhiteSpace(brandName))
            {
                var newBrand = new Brand { Name = brandName };

                using (var context = new FstoreContext())
                {
                    context.Brands.Add(newBrand);
                    context.SaveChanges();
                }

                ((ProductViewModel)DataContext).Brands.Add(newBrand);
                ((ProductViewModel)DataContext).textboxItem.Brand = newBrand;
            }
        }

        private void AddNewCategory_Click(object sender, RoutedEventArgs e)
        {
            string categoryName = Microsoft.VisualBasic.Interaction.InputBox("Enter new Category Name:", "New Category");
            if (!string.IsNullOrWhiteSpace(categoryName))
            {
                var newCategory = new Category { Name = categoryName };

                using (var context = new FstoreContext())
                {
                    context.Categories.Add(newCategory);
                    context.SaveChanges();
                }

                ((ProductViewModel)DataContext).Categories.Add(newCategory);
                ((ProductViewModel)DataContext).textboxItem.Category = newCategory;
            }
        }
    }
}
