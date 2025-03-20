using System.Windows;
using System.Windows.Input;

namespace FinalProject.Views.WarehouseManager.Supplier
{
    /// <summary>
    /// Interaction logic for UpdateSupplier.xaml
    /// </summary>
    public partial class UpdateSupplier : Window
    {
        public UpdateSupplier(FinalProject.Models.Supplier supplier)
        {
            InitializeComponent();
            txtTaxId.Text = supplier.TaxId;
            txtName.Text = supplier.Name;
            txtAddress.Text = supplier.Address;
            txtEmail.Text = supplier.Email;
            txtPhoneNumber.Text = supplier.PhoneNumber;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            Application.Current.Windows[0].Opacity = 1;
            Application.Current.Windows[0].IsHitTestVisible = true;

        }

        private void Border_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            DragMove(); // Kéo popup
        }
    }
}
